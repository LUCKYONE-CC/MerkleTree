using System.Security.Cryptography;
using System.Text;

namespace MerkleTree
{
    public static class Functions
    {
        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public static string GetMerkleRoot(List<byte[]> elements, int count = 1)
        {
            List<string> hashes = new List<string>();

            if(count == 1)
            {
                foreach (var element in elements)
                {
                    hashes.Add(ComputeSha256Hash(Encoding.UTF8.GetString(element)));
                }
                count++;
            }
            else
            {
                if(elements.Count == 1)
                {
                    return Encoding.ASCII.GetString(elements[0]);
                }
                string lastHash = "";
                if(elements.Count % 2 == 1)
                {
                    lastHash = Encoding.ASCII.GetString(elements[elements.Count - 1]);
                }

                for(int i = 0; i <= elements.Count - 3; i+=2)
                {
                    string newHash = ComputeSha256Hash(Encoding.ASCII.GetString(elements[i]) + Encoding.ASCII.GetString(elements[i + 1]));
                    hashes.Add(newHash);
                }

                string newLastHash = ComputeSha256Hash(Encoding.ASCII.GetString(elements[elements.Count - 2]) + Encoding.ASCII.GetString(elements[elements.Count - 1]) + lastHash);
                hashes.Add(newLastHash);
            }

            List<byte[]> byteHashes = new List<byte[]>();
            hashes.ForEach(hash => byteHashes.Add(Encoding.ASCII.GetBytes(hash)));

            return GetMerkleRoot(byteHashes, count);
        }
    }
}