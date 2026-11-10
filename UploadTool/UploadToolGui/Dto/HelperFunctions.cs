using UploadToolGui.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UploadToolGui.Dto
{
    public class HelperFunctions<ID>
    {
        public async Task<ListItem[]> GetItemList(Func<Task<Dictionary<ID, string>>> GetSummary)
        {
            var objs = await GetSummary();
            List<ListItem> items = new List<ListItem>(objs.Count + 1);

            items.AddRange(objs.Select(x => new ListItem(x.Key, x.Value)).ToArray());
            items.Add(new ListItem(default(ID), "[Nieuw]"));

            return items.ToArray();
        }

        public async Task SavePicture(
            IPictureDto<ID> pic, 
            Image image, 
            bool imgChanged, 
            string tagName,
            string orgPictureName, 
            ImageFormat format, 
            Func<string, Task> removePicture,
            Func<Stream, bool, Task> save,
            Func<string, bool, bool, Task> copyPicture
            )
        {
            if (imgChanged)
            {
                using Stream stream = new MemoryStream();
                image.Save(stream, format);
                stream.Position = 0;

                if (orgPictureName != null && orgPictureName != tagName &&
                    !await pic.TagNameExistOutsideId(orgPictureName))
                {
                    await removePicture(orgPictureName);
                }

                await save(stream, true);
            }
            else if (orgPictureName != tagName)
            {
                bool dupOld = await pic.TagNameExistOutsideId(orgPictureName);
                await copyPicture(orgPictureName, true, !dupOld);
            }
        }
    }
}
