namespace MvcExample.Models.Players
{
    public class PlayerKey
    {
        public PlayerKey() {}
        public PlayerKey(int key) { PlayerId = key; }

        public int PlayerId { get; set; }
    }
}