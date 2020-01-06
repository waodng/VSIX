// Guids.cs
// MUST match guids.h
using System;

namespace Company.ZenCoding
{
    static class GuidList
    {
        public const string guidZenCodingPkgString = "2763c8b2-29f7-4628-95e2-1db3186d8fe8";
        public const string guidZenCodingCmdSetString = "2b849bac-6054-4f22-86ef-44aa796179c0";

        public static readonly Guid guidZenCodingCmdSet = new Guid(guidZenCodingCmdSetString);
    };
}