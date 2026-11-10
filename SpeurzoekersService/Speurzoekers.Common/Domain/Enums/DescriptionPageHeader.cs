using System.ComponentModel;

namespace Speurzoekers.Common.Domain.Enums
{
    public enum DescriptionPageHeader : byte
    {
        [Description("Weetje")]
        Weetje = 0,
        [Description("Herkennen van de soort")]
        Herkenning = 1,
        [Description("Voortplanting")]
        Voortplanting = 2,
        [Description("Voorkomen en voedsel")]
        VoorkomenEnVoedsel = 3,
        [Description("Voorkomen")]
        Voorkomen = 4
    }
}
