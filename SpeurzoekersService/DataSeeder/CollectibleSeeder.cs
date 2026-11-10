using Speurzoekers.Common.Domain.Enums;
using Speurzoekers.Data;
using Speurzoekers.Data.Entities;
using Speurzoekers.Data.Entities.Avatar;
using Speurzoekers.Data.Entities.Collectible;
using System.Linq;
using System.Threading.Tasks;

namespace DataSeeder
{
    public static class CollectibleSeeder
    {
        public static int DefaultAvatarPrice = 5000;

        public static Task SeedCollectibleTypes(SpeurzoekersDbContext context)
        {
            CollectibleTypeEntity[] collectibles = new[]
            {
                Aalscholver,
                Egel
            };

            return context.CollectibleTypes.AddRangeAsync(collectibles);
        }

        public static Task SeedAvatars(SpeurzoekersDbContext context)
        {
            AvatarTypeEntity[] avatars = new[]
            {
                DasAvatar,
                RansuilAavatar
            };

            return context.AvatarTypes.AddRangeAsync(avatars);
        }

        public static CollectibleTypeEntity Aalscholver => new CollectibleTypeEntity
        {
            TagName = "Aalscholver-10198.png",
            Name = "Aalscholver",
            Rarity = CollectibleRarity.LessCommon,
            Points = 153,
            QuestionPoints = 23,
            QuestionText = "Hoeveel vis eet ik gemiddeld per dag?",
            MonthsAvailable = EnumExtensions.GetAllYear(),
            Biotopes = (Biotope)122,
            CollectibleTypeChoices = new CollectibleTypeChoiceEntity[]
            {
                new CollectibleTypeChoiceEntity
                {
                    Text = "1 kilo",
                    IsAnswer = false
                },
                new CollectibleTypeChoiceEntity
                {
                    Text = "2 kilo",
                    IsAnswer = false
                },
                new CollectibleTypeChoiceEntity
                {
                    Text = "500 gram",
                    IsAnswer = true
                }
            },
            CollectibleTypeTexts = AalscholverTexts.Select(x => new CollectibleTypeTextEntity
            { 
                TextId = x.TextId,
                Text = x.Text
            }).ToArray()
        };

        public static DescriptionTextEntity[] AalscholverTexts => new DescriptionTextEntity[] 
        {
            new DescriptionTextEntity{
                Text = "**Weetje**\n\nGemiddeld eet ik 500 gram vis per dag.Ik kan wel 10 meter diep duiken. Het diepterecord staat op 75 meter. Vissen slik ik in zijn geheel door. Dikke beenderen en graten verten niet en spuug ik weer uit in een braakbal. De bomen en struiken rondom het nest zijn vaak dood. In kolonies wordt er zoveel gepoept dat de bomen en struiken daar niet tegen kunnen.",
                TextId = DescriptionPageHeader.Weetje
            },
            new DescriptionTextEntity{
                Text= "**Uiterlijk**\n\nIk ben een grote zwarte vogel. In het broedseizoen heb ik een witte kop en wit bij mijn poten. Vrouwtjes hebben dat niet. Aan mijn poten zitten zwemvliezen. Mijn snavel is geel met aan het einde een haak.\n\n"+
                "**Gedrag**\n\nMisschien heb je me wel eens langs het water zien zitten met mijn vleugels gespreid. Dit doe ik om mijn vleugels te laten drogen. Ik ben dol op vis. Deze vang ik door uit de lucht in het water te duiken, of als ik zwem een duik te nemen.\n\n"+
                "**Grootte:**\n\n80-100 cm, spanwijdte 130-160 cm\n\n**Geluid**\n\nIn kolonies ben ik erg luidruchtig en laat ik een rauw *rraaaahhh* horen. Buiten de kolonie laat ik me niet vaak horen.",
                TextId = DescriptionPageHeader.Herkenning
            },
            new DescriptionTextEntity{
                Text= "**Nest**\n\nMijn nest is groot en slordig gebouwd van grote takken en gemaakt in bomen of struiken, veelal op eilandjes. Zo kunnen roofdieren niet bij het nest. Soms broed ik ook op de grond of in het riet.\n\n**Broedperiode**\n\nSoms al in december tot eind augustus\n\n**Aantal nesten per jaar**\n\n1 - 2\n\n**Aantal eieren**\n\n3-4\n\n**Uitbroeden van de eieren**\n\n27-31 dagen\n\n**Uitvliegen**\n\n48-50 dagen\n\n**Bijzonderheden**\n\nKolonies zijn meestal in de buurt van visrijk water. Zo hoef ik niet ver te vliegen om vis te vangen voor mijn jongen.",
                TextId = DescriptionPageHeader.Voortplanting
            },
            new DescriptionTextEntity{
                Text= "**Waar leef ik?**\n\nIk broed in gebieden met rijk viswater als IJsselmeer, Waddeneilanden,Deltagebied, maar ook langs rivieren en moerasgebieden. Buiten debroedperiode ben ik ook op andere wateren te vinden, bij visvijvers en in de stad.\n\n**Wat eet ik?**\n\nIk eet allerlei soorten vis.\n\n**Waar breng ik de winter door?**\n\nIn de winter blijf ik in Nederland, alleen bij strenge vorst trek ik naar Zuidwest Europa.\n\n**Waar kom ik voor in Nederland?**\n\nJe komt me in heel Nederland in de buurt van water tegen, maar ik broed maar op een paar plekken in het land.",
                TextId = DescriptionPageHeader.VoorkomenEnVoedsel
            }
        };

        public static AvatarTypeEntity DasAvatar => new AvatarTypeEntity
        {
            Index = 1,
            TagName = "Das-11049.ori.png",
            Name = "Das",
            Points = DefaultAvatarPrice,
            AvatarTypeTexts = DasTexts.Select(x => new AvatarTypeTextEntity 
            { 
                TextId = x.TextId,
                Text = x.Text
            }).ToArray()
        };

        public static DescriptionTextEntity[] DasTexts => new DescriptionTextEntity[] 
        {
            new DescriptionTextEntity{
                Text = "Ik leef in een burcht. Deze bestaat uit allerlei gangen en kamers. Soms kan een burcht wel een paar honderd meter groot zijn. Een mannetje wordt een beer genoemd een vrouwtje een zeug.",
                TextId = DescriptionPageHeader.Weetje
            },
            new DescriptionTextEntity{
                Text= "**Uiterlijk**\n\n"+
                    "Ik heb een spitse witte kop met 2 zwarte lengte strepen. Mijn rug is grijs en mijn poten en onderlichaam is zwart en heb een brede staart.\n\n"+
                    "**Gedrag**\n\n"+
                    "Overdag slaap ik onder de grond in een burcht. Als het donker begint te worden kom ik tevoorschijn en ga ik op zoek naar eten. Ik ben een roofdier.\n\n"+
                    "**Grootte**\n\n"+
                    "65-80 cm\n\n"+
                    "**Geluid**\n\n"+
                    "Vaak zwijgzaam. Ze maken verschillende geluiden van zwaar gegrom tot korte hoge piepgeluiden.\n",
                TextId = DescriptionPageHeader.Herkenning
            },
            new DescriptionTextEntity{
                Text= "**Jongen**\n\n" +
                        "In februari - maart worden de jongen naakt en blind geboren. Na 5 weken gaan de ogen open. 10 weken lang krijgen ze melk van de moeder. In mei komen de jonge dassen ook boven de grond.",
                TextId = DescriptionPageHeader.Voortplanting
            },
            new DescriptionTextEntity{
                Text= "**Waar leef ik?**\n\n" +
                        "Ik leef in bosachtige gebieden met in de buurt weilanden, akkers of andere open gebieden. Maar ook langs rivieren, en kleinschalig weidelandschap met houtwallen en bosjes. Ik moet wel een burcht kunnen maken, dus de grond moet niet te nat zijn.\n\n" +
                        "**Voedsel**\n\n" +
                        "Alles wat ik onderweg tegenkom; Insecten, slakken, wormen, pissebedden, spinnen en andere kleine diertjes, soms ook eieren van vogels, maar ook bessen, zaden en landbouwgewassen.\n\n" +
                        "**Waar kom ik voor in Nederland?**\n\n" +
                        "Op de drogere zandgronden, zoals de Utrechtse Heuvelrug, Veluwe, Drenthe, Overijssel, Gelderland, Brabant en Limburg.",
                TextId = DescriptionPageHeader.VoorkomenEnVoedsel
            }
        };

        public static CollectibleTypeEntity Egel => new CollectibleTypeEntity
        {
            TagName = "Egel-11138.ori.png",
            Name = "Egel",
            Rarity = CollectibleRarity.Common,
            Points = 45,
            QuestionPoints = 16,
            QuestionText = "Hoeveel stekels heeft een egel gemiddeld?",
            MonthsAvailable = (CalenderMonth)1016,
            Biotopes = (Biotope)967,
            CollectibleTypeChoices = new CollectibleTypeChoiceEntity[]
            {
                new CollectibleTypeChoiceEntity
                {
                    Text = "1000-2000",
                    IsAnswer = false
                },
                new CollectibleTypeChoiceEntity
                {
                    Text = "3000-5000",
                    IsAnswer = false
                },
                new CollectibleTypeChoiceEntity
                {
                    Text = "7000-8000",
                    IsAnswer = true
                }
            },
            CollectibleTypeTexts = EgelTexts.Select(x => new CollectibleTypeTextEntity
            {
                TextId = x.TextId,
                Text = x.Text
            }).ToArray()
        };
        public static DescriptionTextEntity[] EgelTexts = new DescriptionTextEntity[] 
        {
            new DescriptionTextEntity{
                Text = "**Weetje**\n\nIn de herfst ga ik in winterslaap. Mijn lichaamstemperatuur daalt van 35 graden naar 10 graden. Ik verlies dan tot wel 30 % van mijn gewicht. Ik ben een echt nachtdier, overdag slaap ik.\n\n**Let op:**\n\nGeef egels geen melk, ze krijgen daar diarree van.",
                TextId = DescriptionPageHeader.Weetje
            },
            new DescriptionTextEntity{
                Text= "**Uiterlijk**\n\nWie kent mij niet? Het meest opvallende aan mijn uiterlijk zijn mijn stekels op mijn lichaam.",
                TextId = DescriptionPageHeader.Herkenning
            },
            new DescriptionTextEntity{
                Text= "**Nestplaats**\n\nGoed verscholen tussen bladeren maak ik een nest. Soms maak ik ook gebruik van speciale egelkasten.\n\n**Jongen**\n\nIn de periode van juni tot oktober krijgt een egel gemiddeld 5 jongen. Soms kunnen dat er ook 10 zijn. Binnen enkele uren verschijnen witte stekels. Deze worden een paar keer vervangen. Pas na 1,5 jaar krijgt de egel zijn echte stekels. De jongen zijn blind en doof als ze worden geboren. Na 2 weken gaan de ogen en oren open. Na 3 weken verlaten de jongen het nest.",
                TextId = DescriptionPageHeader.Voortplanting
            },
            new DescriptionTextEntity{
                Text= "**Waar leef ik?**\n\nIk leef in tuinen, parken en bossen met veel ondergroei, waar ik me goed kan verbergen.\n\n**Wat eet ik?**\n\nInsecten, slakken, wormen, pissebedden, spinnen en andere kleine diertjes, soms ook eieren van vogels, maar ook bessen vind ik lekker.\n\n**Wanneer kun je me zien?**\n\nVanaf november tot mei ben ik in winterslaap. De rest van het jaar kun je me vooral ’s nachts zien. Overdag slaap ik.\n\n**Waar kom ik voor in Nederland?**\n\nIk kom overal in Nederland voor waar groene beschutte plekken zijn, dus ook in dorpen en steden.",
                TextId = DescriptionPageHeader.VoorkomenEnVoedsel
            }
        };
        public static AvatarTypeEntity RansuilAavatar => new AvatarTypeEntity
        {
            Index = 2,
            TagName = "Ransuil-10588.ori.png",
            Name = "Ransuil",
            Points = DefaultAvatarPrice,
            AvatarTypeTexts = RansuilTexts.Select(x => new AvatarTypeTextEntity
            {
                TextId = x.TextId,
                Text = x.Text
            }).ToArray()
        };

        public static DescriptionTextEntity[] RansuilTexts = new DescriptionTextEntity[] 
        {
            new DescriptionTextEntity{
                Text = "**Bijzonderheden**\n\n" +
                "In een jaar met veel veldmuizen (hoofdvoedsel van de ransuil) leggen ze meer eieren, in een slecht muizen jaar willen ze soms niet broeden.",
                TextId = DescriptionPageHeader.Weetje
            },
            new DescriptionTextEntity{
                Text= "**Uiterlijk**\n\n" +
                "Wat bij mij opvalt, is mijn slanke lichaam met pluimen op mijn kop. Nee, dit zijn geen oren, die zie je niet. Ik heb oranjegele pupillen en verder heb ik een bruingrijs gevlekt verenkleed met een crèmekleurige buik met zwarte lengte strepen.\n\n" +
                "**Gedrag**\n\n" +
                "In de winter leef ik vaak in groepen. We verzamelen overdag op een vaste plek. Zo'n plek noem je ook wel roestplek. In de schemering word ik  actief, overdag zit ik tegen een boomstam aan verscholen.\n\n" +
                "**Grootte**\n\n" +
                "35-37 cm, spanwijdte 90-100 cm\n\n" +
                "**Roep**\n\n" +
                "Mannetje een laag 'hoe', vrouwtje een schel 'weeh'.",
                TextId = DescriptionPageHeader.Herkenning
            },
            new DescriptionTextEntity{
                Text= "**Nest**\n\n" +
                "Als nest gebruik ik vaak oude nesten van kraaien of eksters. Het liefst een nest in een naaldboom.\n\n" +
                "**Aantal nesten per jaar**\n\n" +
                "1\n\n" +
                "**Broedperiode**\n\n" +
                "Eind maart -- half april\n\n" +
                "**Aantal eieren**\n\n" +
                "4-6 eieren\n\n" +
                "**Uitbroeden van de eieren**\n\n" +
                "26-28 dagen\n\n" +
                "**Uitvliegen**\n\n" +
                "Verlaten na 21 dagen het nest en kunnen pas bij 40-42 dagen vliegen.",
                TextId = DescriptionPageHeader.Voortplanting
            },
            new DescriptionTextEntity{
                Text= "**Waar leef ik?**\n\n" +
                "In heel veel verschillende gebieden kom je me tegen, zoals in parken, tuinen, agrarisch landschap, open bos, bosranden, duinen en  heidegebieden.\n\n" +
                "**Voedsel**\n\n" +
                "Muizen, maar ook kleine vogels.\n\n" +
                "**Waar kom ik voor in Nederland?**\n\n" +
                "In grote delen van Nederland, behalve in open gebieden.",
                TextId = DescriptionPageHeader.VoorkomenEnVoedsel
            }
        };
    }
}
