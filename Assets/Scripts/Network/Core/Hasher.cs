﻿namespace Networking
{
    public static class Hasher
    {
        // Count of bytes in hash
        // WARNING: DON'T TOUCH THIS (line 45)
        private const int capacity = 4;

        //TODO: Need test for table (no repeting)
        public static int[] hash_table = new int[]{
            098, 006, 085, 150, 036, 023, 112, 164, 135, 207, 169, 005, 026, 064, 165, 219,
            061, 020, 068, 089, 130, 063, 052, 102, 024, 229, 132, 245, 080, 216, 195, 115,
            090, 168, 156, 203, 177, 120, 002, 190, 188, 007, 100, 185, 174, 243, 162, 010,
            237, 018, 253, 225, 008, 208, 172, 244, 255, 126, 101, 079, 145, 235, 228, 121,
            123, 251, 067, 250, 161, 000, 107, 097, 241, 111, 181, 082, 249, 033, 069, 055,
            059, 153, 029, 009, 213, 167, 084, 093, 030, 046, 094, 075, 151, 114, 073, 222,
            197, 096, 210, 045, 016, 227, 248, 202, 051, 152, 252, 125, 081, 206, 215, 186,
            039, 158, 178, 187, 131, 136, 001, 049, 050, 017, 141, 091, 047, 129, 060, 099,
            154, 035, 086, 171, 105, 034, 038, 200, 147, 058, 077, 118, 173, 246, 076, 254,
            133, 232, 196, 144, 198, 124, 053, 004, 108, 074, 223, 234, 134, 230, 157, 139,
            189, 205, 199, 128, 176, 019, 211, 236, 127, 192, 231, 070, 233, 088, 146, 044,
            183, 201, 022, 083, 013, 214, 116, 109, 159, 032, 095, 226, 140, 220, 057, 012,
            221, 031, 209, 182, 143, 092, 149, 184, 148, 062, 113, 065, 037, 027, 106, 166,
            003, 014, 204, 072, 021, 041, 056, 066, 028, 193, 040, 217, 025, 054, 179, 117,
            238, 087, 240, 155, 180, 170, 242, 212, 191, 163, 078, 218, 137, 194, 175, 110,
            043, 119, 224, 071, 122, 142, 042, 160, 104, 048, 247, 103, 015, 011, 138, 239, 004
        };

        public static int GetHash(string value)
        {
            var code = 0;
            var array = System.Text.Encoding.ASCII.GetBytes(value);

            for (int rank = 0; rank < capacity; rank++)
            {
                int h = hash_table[(array[0] + rank) % 256];

                for (int i = 1; i < array.Length; i++)
                {
                    h = hash_table[h ^ array[i]];
                }

                // ONLY FOR CAPACITY 4
                code |= h << rank;
            }

            return code;
        }
    }
}
