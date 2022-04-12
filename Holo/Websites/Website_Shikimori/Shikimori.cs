using Holo.Websites.Website_Shikimori.Structs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Holo.Websites.Website_Shikimori
{
    public class Shikimori
    {
        public List<string> ShikimoriUsers = new List<string>();

        public List<AnimePerson> Persons = new List<AnimePerson>();
        public List<Anime> Animes = new List<Anime>(); //Ost End Op Coub JapCharact DescrCharact

        public List<Anime> YearOfAnime = new List<Anime>();
        public List<Anime> FillAnimeByPoster = new List<Anime>();
        public List<Anime> FillAnimeByScreenshot = new List<Anime>();
        public List<Anime> FillAnimeByJapane = new List<Anime>();
        public List<Anime> FillAnimeByDisappearance = new List<Anime>();
        public List<Anime> FillAnimeByDescription = new List<Anime>();
        public List<Anime> FillAnimeByAnagramm = new List<Anime>();
        public List<AnimePerson> FillCharacterByImage = new List<AnimePerson>();

        Random rand = new Random(Guid.NewGuid().GetHashCode());
        public const bool DoDelete = false;

        public void FillLists()
        {
            foreach (string shiki_url in ShikimoriUsers)
            {
                UserAnime[] userAnimes = Main_Shikimori.GetUserAnimeList(shiki_url, Main_Shikimori.CurrentAnimeStatus);
                for (int i = 0; i < userAnimes.Length; i++)
                {
                    Anime anime = Main_Shikimori.GetAnimeInformation(userAnimes[i]);
                    if (anime != null) anime.UserOwner = shiki_url;
                    Main_Shikimori.GetAnimeScreenshots(anime);
                    Animes.Add(anime);

                    if (anime.AiredOn.HasValue) YearOfAnime.Add(anime);
                    if (anime.Image != null && anime.Image.Preview != "") FillAnimeByPoster.Add(anime);
                    if (anime.Screenshots != null && anime.Screenshots.Length > 0) FillAnimeByScreenshot.Add(anime);
                    if (anime.Japanese != null && anime.Japanese.Length > 0) FillAnimeByJapane.Add(anime);
                    if (anime.Russian != "") FillAnimeByDisappearance.Add(anime);
                    if (anime.Description != "") FillAnimeByDescription.Add(anime);
                    if (anime.Russian != "") FillAnimeByAnagramm.Add(anime);

                    AnimePerson[] persons = Main_Shikimori.GetAnimeCharacters(anime.ID);

                    for (int j = 0; j < persons.Length; j++)
                    {
                        persons[j].Anime = anime;
                        if (persons[j].Roles.Contains("Main") || (Main_Shikimori.UseSecondaryCharacers && persons[j].Roles.Contains("Supporting")))
                            if (persons[j].Character != null)
                                if (Persons.Where(p => p.Character.ID == persons[j].Character.ID).Count() == 0)
                                {
                                    Persons.Add(persons[j]);
                                    if (persons[j].Character.Image != null && persons[j].Character.Image.Preview != "") FillCharacterByImage.Add(persons[j]);
                                }
                    }
                    if (Animes.Where(p => p.ID == anime.ID).Count() == 0)
                    {
                        Animes.Add(anime);
                    }
                }
            }
        }

        public Anime GetRandomAnime()
        {
            var temp = Animes[rand.Next(0, Animes.Count)];
            return temp;
        }

        public Anime GetYearOfAnime()
        {
            var temp = YearOfAnime[rand.Next(0, YearOfAnime.Count)];
            if (DoDelete) YearOfAnime.Remove(temp);
            return temp;
        }

        public AnimePerson GetCharacterByJapanese()
        {
            var temp = Persons[rand.Next(0, Persons.Count)];
            return temp;
        }

        public AnimePerson GetCharacterByImage()
        {
            var temp = FillCharacterByImage[rand.Next(0, FillCharacterByImage.Count)];
            if (DoDelete) FillCharacterByImage.Remove(temp);
            return temp;
        }

        public AnimePerson GetCharacterByDescription()
        {
            var temp = Persons[rand.Next(0, Persons.Count)];
            if (DoDelete) Persons.Remove(temp);
            return temp;
        }

        public Anime GetAnimeByOst()
        {
            var temp = Animes[rand.Next(0, Animes.Count)];
            return temp;
        }

        public Anime GetAnimeByPoster()
        {
            var temp = FillAnimeByPoster[rand.Next(0, FillAnimeByPoster.Count)];
            if (DoDelete) FillAnimeByPoster.Remove(temp);
            return temp;
        }

        public Anime GetAnimeByScreenshot()
        {
            var temp = FillAnimeByScreenshot[rand.Next(0, FillAnimeByScreenshot.Count)];
            if (DoDelete) FillAnimeByScreenshot.Remove(temp);
            return temp;
        }

        public Anime GetAnimeByOpening()
        {
            var temp = Animes[rand.Next(0, Animes.Count)];
            return temp;
        }

        public Anime GetAnimeByJapane()
        {
            var temp = FillAnimeByJapane[rand.Next(0, FillAnimeByJapane.Count)];
            if (DoDelete) FillAnimeByJapane.Remove(temp);
            return temp;
        }

        public Anime GetAnimeByEnding()
        {
            var temp = Animes[rand.Next(0, Animes.Count)];
            return temp;
        }

        public Anime GetAnimeByDisappearance()
        {
            var temp = FillAnimeByDisappearance[rand.Next(0, FillAnimeByDisappearance.Count)];
            if (DoDelete) FillAnimeByDisappearance.Remove(temp);
            return temp;
        }

        public Anime GetAnimeByDescription()
        {
            var temp = FillAnimeByDescription[rand.Next(0, FillAnimeByDescription.Count)];
            if (DoDelete) FillAnimeByDescription.Remove(temp);
            return temp;
        }

        public Anime GetAnimeByAnagramm()
        {
            var temp = FillAnimeByAnagramm[rand.Next(0, FillAnimeByAnagramm.Count)];
            if (DoDelete) FillAnimeByAnagramm.Remove(temp);
            return temp;
        }

        public Anime GetAnimeByCoub()
        {
            var temp = Animes[rand.Next(0, Animes.Count)];
            return temp;
        }
    }
}
