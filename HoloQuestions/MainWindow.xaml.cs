using HoloQuestions.Managers;
using HoloQuestions.SIGame.Elements;
using HoloQuestions.SIGame.Themes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HoloQuestions
{
    public partial class MainWindow : Window
    {
        HashSet<ITheme> AllThemes = new HashSet<ITheme>()
        {
            new Theme_AnimeByAnagramm()
        };

        HashSet<IFinal> AllFinals = new HashSet<IFinal>()
        {

        };

        public MainWindow()
        {
            InitializeComponent();
        }

        void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShikiProfilesManager.Load();
            ShowShikiProfiles();
            foreach (ITheme theme in AllThemes)
            {
                CheckBox checkBox = new CheckBox()
                {
                    Content = theme,
                    IsChecked = false
                };
                Themes_listbox.Items.Add(checkBox);
            }
        }

        void Window_Closed(object sender, EventArgs e)
        {
            
        }

        void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        void Minimize_button_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        void Exit_button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        void CreatePack_button_Click(object sender, RoutedEventArgs e)
        {
            /*
            Имя файла
            Ограничение
            -
            Статус аниме
            Второстепенные персы
            Очистить кэш (вес)
            ВСЁ МОЖЕТ ПОВТОРЯТЬСЯ!
            */
            HashSet<ITheme> Themes = new HashSet<ITheme>();

            foreach (CheckBox checkBox in Themes_listbox.Items)
            {
                if (checkBox.IsChecked.Value) Themes.Add((ITheme)checkBox.Content);
            }

            HashSet<IFinal> Finals = AllFinals;

            if (Themes.Count != 0)
            {
                ProgressWindow progressWindow = new ProgressWindow();
                progressWindow.Start(Title_textbox.Text, ((int)Difficulty_slider.Value).ToString(), Version_textbox.Text, (int)Rounds_slider.Value, (int)Themes_slider.Value, (int)Questions_slider.Value, (int)Finals_slider.Value, Themes, Finals);
                progressWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Вы не указали ни одной темы!", "Не получилось", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void AddShiki_button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Shiki_textbox.Text) && !string.IsNullOrWhiteSpace(Shiki_textbox.Text))
            {
                if (ShikiProfilesManager.AddProfile(Shiki_textbox.Text))
                {
                    Shiki_textbox.Text = "";
                    ShowShikiProfiles();
                }
            }
        }

        void RemoveShiki_button_Click(object sender, RoutedEventArgs e)
        {
            if (Shiki_listbox.SelectedIndex != -1)
            {
                ShikiProfilesManager.RemoveProfile((ShikiProfile)((CheckBox)Shiki_listbox.SelectedItem).Content);
                Shiki_listbox.Items.RemoveAt(Shiki_listbox.SelectedIndex);
            }
        }

        void ShowShikiProfiles()
        {
            Shiki_listbox.Items.Clear();

            for (int i = 0; i < ShikiProfilesManager.ShikimoriProfiles.Count; i++)
            {
                CheckBox checkBox = new CheckBox()
                {
                    Content = ShikiProfilesManager.ShikimoriProfiles[i],
                    IsChecked = ShikiProfilesManager.ShikimoriProfiles[i].Selected
                };
                checkBox.Click += ShikiProfile_Click;
                Shiki_listbox.Items.Add(checkBox);
            }
        }

        void ShikiProfile_Click(object sender, RoutedEventArgs e)
        {
            ((ShikiProfile)((CheckBox)sender).Content).InvertSelected();
        }
    }
}
