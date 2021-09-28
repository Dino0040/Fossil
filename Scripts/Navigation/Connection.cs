namespace Fossil
{
    [System.Serializable]
    public class Connection
    {
        public Node node;
        public float distance;

        public Connection(Node node, float distance)
        {
            this.node = node;
            this.distance = distance;
        }
    }
}