using HoloQuestions.Managers;
using HoloQuestions.SIGame;
using HoloQuestions.SIGame.Elements;
using System.Collections.Generic;
using System.Windows;

namespace HoloQuestions
{
    public partial class ProgressWindow : Window
    {
        /*
         У каждой темы есть особый статичный массив необходимых данных, формируется по условию присутствия поля
             
             */

        public void Start(string title, string difficulty, string version, int rounds_count, int themes_count, int questions_count, int finals_count, HashSet<ITheme> themes, HashSet<IFinal> finals)
        {
            Ready_button.IsEnabled = false;
            Stop_button.IsEnabled = true;

            SIGamePack pack = SIGameManager.CreatePack(title, difficulty, version, rounds_count, themes_count, questions_count, finals_count, 100, 100, themes, finals);
            SIGameManager.ExportPack(pack);

            PrintGood("Пак создан!");

            Ready_button.IsEnabled = true;
            Stop_button.IsEnabled = false;
        }

        void Stop()
        {

        }
    }
}
