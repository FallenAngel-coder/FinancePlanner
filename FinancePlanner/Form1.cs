namespace FinancePlanner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Використовуємо окремий клас дизайнера для створення кнопок і логіки меню
            MainMenuDesigner menuDesigner = new MainMenuDesigner();
            menuDesigner.Setup(this);
        }
    }
}
