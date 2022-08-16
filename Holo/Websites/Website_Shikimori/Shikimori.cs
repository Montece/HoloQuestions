using Holo.Websites.Website_Shikimori.Structs;
using Holo.Websites.Website_Themes_Moe;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Holo.Websites.Website_Shikimori
{
    public class Shikimori
    {
        private readonly List<string> ShikimoriUsers = new List<string>();

        private readonly List<AnimePerson> AllPersons = new List<AnimePerson>();
        private readonly List<Anime> AllAnimes = new List<Anime>();
        private readonly CategotyList<Anime> YearOfAnime = new CategotyList<Anime>();
        private readonly CategotyList<Anime> FillAnimeByPoster = new CategotyList<Anime>();
        private readonly CategotyList<Anime> FillAnimeByScreenshot = new CategotyList<Anime>();
        private readonly CategotyList<Anime> FillAnimeByJapane = new CategotyList<Anime>();
        private readonly CategotyList<Anime> FillAnimeByDisappearance = new CategotyList<Anime>();
        private readonly CategotyList<Anime> FillAnimeByDescription = new CategotyList<Anime>();
        private readonly CategotyList<Anime> FillAnimeByAnagramm = new CategotyList<Anime>();
        private readonly CategotyList<Anime> FillAnimeByCoub = new CategotyList<Anime>();
        private readonly CategotyList<Anime> FillAnimeByOpening = new CategotyList<Anime>();
        private readonly CategotyList<Anime> FillAnimeByEnding = new CategotyList<Anime>();
        private readonly CategotyList<AnimePerson> FillCharacterByImage = new CategotyList<AnimePerson>();
        private readonly CategotyList<AnimePerson> FillCharacterByJapanese = new CategotyList<AnimePerson>();
        private readonly CategotyList<AnimePerson> FillCharacterByDescription = new CategotyList<AnimePerson>();

        public void AddUser(string user)
        {
            ShikimoriUsers.Add(user);
        }

        public bool HasUser(string user)
        {
            return ShikimoriUsers.Contains(user);
        }

        public int GetUsersCount()
        {
            return ShikimoriUsers.Count;
        }

        public void FillLists()
        {
            foreach (string shiki_url in ShikimoriUsers)
            {
                List<UserAnime> userAnimes = Main_Shikimori.GetUserAnimeList(shiki_url, Main_Shikimori.CurrentAnimeStatus);
                for (int i = 0; i < userAnimes.Count; i++)
                {
                    Anime anime = Main_Shikimori.GetAnimeInformation(userAnimes[i]);
                    if (anime != null) anime.UserOwner = shiki_url;
                    Main_Shikimori.GetAnimeScreenshots(anime);
                    FillAnimeByCoub.Add(anime);

                    if (anime.AiredOn.HasValue) YearOfAnime.Add(anime);
                    if (anime.Image != null && !string.IsNullOrEmpty(anime.Image.Preview)) FillAnimeByPoster.Add(anime);
                    if (anime.Screenshots != null && anime.Screenshots.Length > 0) FillAnimeByScreenshot.Add(anime);
                    if (anime.Japanese != null && anime.Japanese.Length > 0) FillAnimeByJapane.Add(anime);
                    if (!string.IsNullOrEmpty(anime.Russian)) FillAnimeByDisappearance.Add(anime);
                    if (!string.IsNullOrEmpty(anime.Description)) FillAnimeByDescription.Add(anime);
                    if (!string.IsNullOrEmpty(anime.Russian)) FillAnimeByAnagramm.Add(anime);

                    AnimePerson[] persons = Main_Shikimori.GetAnimeCharacters(anime.ID);
                    for (int j = 0; j < persons.Length; j++)
                    {
                        persons[j].Anime = anime;
                        if (persons[j].Roles.Contains("Main") || (Main_Shikimori.UseSecondaryCharacers && persons[j].Roles.Contains("Supporting")))
                            if (persons[j].Character != null)
                                if (AllPersons.Where(p => p.Character.ID.Equals(persons[j].Character.ID)).Count().Equals(0))
                                {
                                    AllPersons.Add(persons[j]);
                                    if (!string.IsNullOrEmpty(persons[j].Character.Japanese) && !string.IsNullOrEmpty(persons[j].Character.Russian)) FillCharacterByJapanese.Add(persons[j]);
                                    if (!string.IsNullOrEmpty(persons[j].Character.Russian)) FillCharacterByDescription.Add(persons[j]);
                                    if (!string.IsNullOrEmpty(persons[j].Character.Russian) && persons[j].Character.Image != null && persons[j].Character.Image.Preview != "") FillCharacterByImage.Add(persons[j]);
                                }
                    }

                    //Заполнение списка опенингов и эндингов
                    anime.OPs = new List<AnimeMusic>();
                    anime.EDs = new List<AnimeMusic>();

                    SearchAnimeResponseDto animeMusic = Main_Themes_Moe.GetAnimeMusic(anime.ID);
                    if (animeMusic != null)
                    {
                        for (int j = 0; j < animeMusic.Themes.Count; j++)
                        {
                            if (animeMusic.Themes[j].ThemeType.Contains("OP"))
                            {
                                anime.OPs.Add(new AnimeMusic()
                                {
                                    URL = animeMusic.Themes[j].mirror.URL
                                });
                                FillAnimeByOpening.Add(anime);
                            }
                            if (animeMusic.Themes[j].ThemeType.Contains("ED"))
                            {
                                anime.EDs.Add(new AnimeMusic()
                                {
                                    URL = animeMusic.Themes[j].mirror.URL
                                });
                                FillAnimeByEnding.Add(anime);
                            }
                        }
                    }

                    //Если нет такого аниме - добавляем его
                    if (AllAnimes.Where(p => p.ID.Equals(anime.ID)).Count().Equals(0)) AllAnimes.Add(anime);
                }
            }
        }

        public Anime GetRandomAnime()
        {
            return AllAnimes.GetRandomElement();
        }

        public AnimePerson GetRandomPerson()
        {
            return AllPersons.GetRandomElement();
        }

        public Anime GetYearOfAnime()
        {
            return YearOfAnime.GetRandomElement();
        }

        public AnimePerson GetCharacterByJapanese()
        {
            return FillCharacterByJapanese.GetRandomElement();
        }

        public AnimePerson GetCharacterByImage()
        {
            return FillCharacterByImage.GetRandomElement();
        }

        public AnimePerson GetCharacterByDescription()
        {
            return FillCharacterByDescription.GetRandomElement();
        }

        public Anime GetAnimeByPoster()
        {
            return FillAnimeByPoster.GetRandomElement();
        }

        public Anime GetAnimeByScreenshot()
        {
            return FillAnimeByScreenshot.GetRandomElement();
        }    

        public Anime GetAnimeByJapane()
        {
            return FillAnimeByJapane.GetRandomElement();
        }

        public Anime GetAnimeByDisappearance()
        {
            return FillAnimeByDisappearance.GetRandomElement();
        }

        public Anime GetAnimeByDescription()
        {
            return FillAnimeByDescription.GetRandomElement();
        }

        public Anime GetAnimeByAnagramm()
        {
            return FillAnimeByAnagramm.GetRandomElement();
        }

        public Anime GetAnimeByCoub()
        {
            return FillAnimeByCoub.GetRandomElement();
        }

        public Anime GetAnimeByOpening()
        {
            return FillAnimeByOpening.GetRandomElement();
        }

        public Anime GetAnimeByEnding()
        {
            return FillAnimeByEnding.GetRandomElement();
        }
    }

    public class CategotyList<T>
    {
        private readonly List<T> FillList = new List<T>();
        private readonly List<T> FillList_Backup = new List<T>();

        public void Add(T element)
        {
            FillList.Add(element);
            FillList_Backup.Add(element);
        }

        public void Remove(T element)
        {
            if (FillList.Contains(element)) FillList.Remove(element);
        }

        public T GetRandomElement()
        {
            if (FillList.Count.Equals(0))
            {
                if (FillList_Backup.Count.Equals(0)) return default;
                T[] temp = new T[FillList_Backup.Count];
                FillList_Backup.CopyTo(temp);
                FillList.AddRange(temp);
            }

            T element = FillList.GetRandomElement();
            Remove(element);
            return element;
        }
    }
}
