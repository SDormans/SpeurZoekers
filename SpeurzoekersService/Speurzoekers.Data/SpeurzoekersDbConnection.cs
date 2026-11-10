using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Speurzoekers.Common.Config;
using Speurzoekers.Common.Repositories;
using Speurzoekers.Data.Repositories;
using System;
using System.IO;
using System.Linq;

namespace Speurzoekers.Data
{
    public class SpeurzoekersDbConnection
    {
        public class SpeurzoekersDbConException : Exception
        {
            public SpeurzoekersDbConException(string message) : base(message)
            { }
        }

        public ICollectibleRepository CollectibleRepository
        {
            get
            {
                if (_collectibleRepository == null)
                {
                    _collectibleRepository = new CollectibleRepository(_con, _mapper, ConfigurationRepository);
                }

                return _collectibleRepository;
            }
        }
        public IRegionRepository RegionRepository
        {
            get
            {
                if (_regionRepository == null)
                {
                    _regionRepository = new RegionRepository(_con, _mapper);
                }

                return _regionRepository;
            }
        }
        public IConfigurationRepository ConfigurationRepository
        {
            get
            {
                if (_configurationRepository == null)
                {
                    _configurationRepository = new ConfigurationRepository(_con);
                }

                return _configurationRepository;
            }
        }
        public IAvatarRepository AvatarRepository
        {
            get
            {
                if (_avatarRepository == null)
                {
                    _avatarRepository = new AvatarRepository(_con, UserRepository, _mapper);
                }

                return _avatarRepository;
            }
        }
        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    var auth = Options.Create(
                        _configuration.GetSection(ConfigurationKeys.AuthenticationSection)
                        .Get<AuthenticationOptions>());
                    _userRepository = new UserRepository(null, null, _mapper, _con, auth);
                }

                return _userRepository;
            }
        }
        public IBlobRepository BlobRepository
        {
            get
            {
                if (_blobRepository == null)
                {
                    CreateBlobRepository();
                }

                return _blobRepository;
            }
        }
        public IActivityRepository ActivityRepository
        {
            get
            {
                if (_activityRepository == null)
                {
                    _activityRepository = new ActivityRepository(_con, _mapper);
                }

                return _activityRepository;
            }
        }

        private readonly string _basePath;
        private readonly IConfigurationRoot _configuration, _localConfiguration;
        private readonly IMapper _mapper;
        private readonly SpeurzoekersDbContext _con;
        private ICollectibleRepository _collectibleRepository;
        private IRegionRepository _regionRepository;
        private IConfigurationRepository _configurationRepository;
        private IAvatarRepository _avatarRepository;
        private IUserRepository _userRepository;
        private IBlobRepository _blobRepository;
        private IActivityRepository _activityRepository;

        /// <summary>Reads configs and set blob and SQL repositories</summary>
        /// <exception cref="SpeurzoekersDbConException">Thrown when unable to connect to the requested DB</exception>
        public SpeurzoekersDbConnection(string basePath, params Profile[] profiles) : this(profiles, basePath)
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlServer(GetConnectionString());
            _con = new SpeurzoekersDbContext(optionsBuilder.Options);
        }

        /// <summary>reads only the config(s)</summary>
        public SpeurzoekersDbConnection(string basePath) : this(new Profile[0], basePath)
        { }

        private SpeurzoekersDbConnection(Profile[] profiles, string basePath)
        {
            _basePath = basePath;

            IConfigurationBuilder builder = new ConfigurationBuilder()
                       .SetBasePath(_basePath)
#if DEBUG
                .AddJsonFile("dbsettings.Debug.json");
#else
                .AddJsonFile("dbsettings.Release.json");
#endif                
            _configuration = builder.Build();

#if DEBUG
            if (File.Exists(Path.Combine(_basePath, "localsettings.json")))
            {
                builder = new ConfigurationBuilder()
                    .SetBasePath(_basePath)
                    .AddJsonFile("localsettings.json");
                _localConfiguration = builder.Build();
            }
#endif            

            MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfiles(profiles);
            });

            _mapper = mappingConfig.CreateMapper();
        }

        /// <summary>Sets the SQL connection according to the config(s)</summary>
        public void SetSqlConnection(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(GetConnectionString());
        }

        /// <summary>Check whether the requested database is able to connect</summary>
        /// <exception cref="SpeurzoekersDbConException">Thrown when unable to connect to the requested DB</exception>
        public void CheckSqlConnection()
        {
            CheckSqlConnection(_con);
        }

        /// <summary>Check whether the requested database is able to connect</summary>
        /// <exception cref="SpeurzoekersDbConException">Thrown when unable to connect to the requested DB</exception>
        public void CheckSqlConnection(SpeurzoekersDbContext context)
        {
            try
            {
                context.Database.CanConnect();
            }
            catch (Exception e)
            {
                throw new SpeurzoekersDbConException($"Can't connect to the requested database ({e.Message}).");
            }
        }

        /// <summary>Check whether the requested database model matches the app model</summary>
        /// <exception cref="SpeurzoekersDbConException">Thrown when unable to connect to the requested DB</exception>
        public void CheckValiditySqlScheme()
        {
            CheckValiditySqlScheme(_con);
        }

        /// <summary>Check whether the requested database model matches the app model</summary>
        /// <exception cref="SpeurzoekersDbConException">Thrown when unable to connect to the requested DB</exception>
        public void CheckValiditySqlScheme(SpeurzoekersDbContext context)
        {
            string[] db = context.Database.GetAppliedMigrations().ToArray();
            string[] model = context.Database.GetMigrations().ToArray();

            if (!db.SequenceEqual(model))
            {
                throw new SpeurzoekersDbConException("Datamodel doesn't match the requested database; " +
                    $"expected {model.Length}, found {db.Length}");
            }
        }

        private void CreateBlobRepository()
        {
            IConfigurationSection blobConfig = _configuration.GetSection(ConfigurationKeys.BlobStorageSection);
            BlobStorage blobStorage = blobConfig.Get<BlobStorage>();

            string path = _localConfiguration?.GetValue<string>(ConfigurationKeys.BlobLocalPathKey);

#if DEBUG
            if (!string.IsNullOrWhiteSpace(path))
            {
                if (path == "\\" || path == "/")
                {
                    path = _basePath;
                }

                path = path.Replace("%pics%", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
                blobStorage.LocalPath = path;

                _blobRepository = new LocalBlobRepository(Options.Create(blobStorage));
            }
            else
#endif 
            {
                _blobRepository = new BlobRepository(Options.Create(blobStorage));
            }
        }

        private string GetConnectionString()
        {
            string con = null;

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    con = _configuration.GetValue<string>(ConfigurationKeys.DbConnectionStringWin);
                    break;
                case PlatformID.Unix:
                    con = _configuration.GetValue<string>(ConfigurationKeys.DbConnectionStringUnix);
                    break;
            }

            if (con == null)
            {
                string platform = Environment.OSVersion.Platform.ToString();
#if DEBUG
                string isDev = "Debug";
#else
                string isDev = "Release";
#endif
                throw new SpeurzoekersDbConException($"No connection string defined for platform {platform} in {isDev} configuration.");
            }
            //else
            return con;
        }
    }
}
