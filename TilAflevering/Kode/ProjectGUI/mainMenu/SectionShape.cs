using System.Windows.Shapes;

namespace mainMenu
{
    public class SectionShape
    {
        public string Name { get; set; }

        public  long ID { get; set; }

        public Shape Shape { get; set; }

        public double Top { get; set; }
        public double Left { get; set; }
    }
}