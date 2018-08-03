
using System;
using System.ComponentModel.DataAnnotations;

namespace EnumSample.Models
{
    public enum MyEnum : byte
    {
        [Display(Name = "Zéro")]
        Zero,

        [Display(Name = "Un")]
        One,

        [Display(Name = "Duex")]
        Two,

        [Display(Name = "Trois")]
        Three,

        [Display(Name = "Quatre")]
        Four,
    }

    [Flags]
    public enum FlagsEnum : byte
    {
        Zero = 0,
        One = 1,
        Two = 2,
        Four = 4,
        Eight = 8,
        Sixteen = 16,
    }
}