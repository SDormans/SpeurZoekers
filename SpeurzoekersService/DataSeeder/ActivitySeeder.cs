using Speurzoekers.Common.Domain.Enums;
using Speurzoekers.Data;
using Speurzoekers.Data.Entities;
using System.Threading.Tasks;

namespace DataSeeder
{
    public static class ActivitySeeder
    {
        public static Task SeedActivities(SpeurzoekersDbContext context)
        {
            ActivityEntity[] acts = new[]
            {
                Loloballen,
                LandjeVeroveren
            };

            return context.Activities.AddRangeAsync(acts);
        }

        public static ActivityEntity Loloballen => new ActivityEntity
        {
            Name = "Loloballen",
            Season = EnumExtensions.GetAllSeasons(),
            TagName1 = "Loloballen-klein-IMG_8335.jpg",
            Text = "Met een lolobal kun je lekker heen en weer stuiteren."
        };

        public static ActivityEntity LandjeVeroveren => new ActivityEntity
        {
            Name = "Landje veroveren",
            Season = Season.Spring | Season.Summer,
            TagName1 = "Landje-veroveren-klein-IMG_8368-1.jpg",
            Text = "Met dit spel is het handig om verschillende kleuren stoepkrijt te hebben.\n\n" +
            "Je tekent één grote cirkel. Deze verdeel je in taartpunten, net zoveel als het aantal spelers." +
            " Elk vak krijgt een kleur. Elke speler kiest een kleur en gaat in dit vak staan met een stuk krijt. Dit is nu zijn/haar vak en kleur.\n\n" +
            "**Starten met het spel** 1 speler zegt; ‘ik verklaar de oorlog aan groen’." +
            " Iedereen die niet in het groene vak staat rent weg. De speler in het groene vak roep ‘STOP’." +
            " Iedereen moet nu stil staan. De speler in het groene vak probeert met de handen de voeten van een andere speler aan te raken, maar moet wel met beide voeten in het groene vak blijven." +
            " Je mag dus ook gaan liggen. Is dat gelukt, dan mag je om de voeten van je medespeler een cirkel tekenen. Dit is nu jouw stukje land." +
            " Kun je geen voeten aan tikken, dan roep je lopen. De spelers lopen richting hun vak, totdat je stop zegt. Kun je nu wel een paar voeten raken." +
            " Zo lang je geen stop zegt, kunnen de kinderen terugkomen in hun eigen vak en zijn ze vrij. Is iedereen vrij of heb je iemand getikt, dan mag jij een kleur roepen." +
            " En zo gaat het spel verder. Heb je een landje verovert, dan mag je ook via dit stukje land de voeten van een ander proberen te tikken. De speler met de meeste stukjes land heeft gewonnen."
        };
    }
}
