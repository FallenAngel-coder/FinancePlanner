using System;
using System.Windows.Forms;

namespace FinancePlanner
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            // Ініціалізуємо БД до запуску вікон
            DatabaseInitializer.Initialize();

            // Відкриваємо форму авторизації
            using (var authForm = new Forms.AuthForm())
            {
                if (authForm.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new Form1());
                }
            }
        }
    }
}