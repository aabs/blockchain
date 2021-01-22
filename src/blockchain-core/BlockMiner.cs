using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace blockchain_core
{
    public class Block
    {
        public byte[] Data { get; set; }
        public string PreviousBlockHash { get; set; }
        public string BlockHash { get; set; }
        public string Nonce { get; set; }
    }

    public static class BlockMiner
    {
        public static Random Random = new Random(DateTime.Now.Millisecond);
        public static string ComputeBlockNonce(Block b, int numberOfLeadingZeroesRequired)
        {
            var nonceGuess = "01234567012345670123456701234567"; // 32 chars
            var leadingZeroesGuessed = 0;
            while (leadingZeroesGuessed < numberOfLeadingZeroesRequired)
            {
                nonceGuess = GuessNonce();
                var bGuess = new Block
                {
                    Data = b.Data,
                    PreviousBlockHash = b.PreviousBlockHash,
                    Nonce = nonceGuess
                };
                leadingZeroesGuessed = LeadingZeroes(ComputeHashForBlock(bGuess));
            }

            return nonceGuess;
        }

        private static string GuessNonce() 
        {
            var buf = new byte[128];
            Random.NextBytes(buf);
            return Convert.ToBase64String(buf);
        }

        public static int LeadingZeroes(string s)
        {
            var result = 0;
            foreach (var c in s)
            {
                if (c != '0')
                {
                    return result;
                }

                result++;
            }

            return result;
        }

        public static string ComputeHashForBlock(Block b)
        {
            var sb = new StringBuilder(ComputeSha256Hash(b.Data));
            sb.Append(b.PreviousBlockHash);
            sb.Append(b.Nonce);
            return ComputeSha256Hash(Encoding.UTF8.GetBytes(sb.ToString()));
        }

        static string ComputeSha256Hash(byte[] rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(rawData);

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
