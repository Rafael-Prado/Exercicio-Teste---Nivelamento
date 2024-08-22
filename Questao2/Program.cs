using Newtonsoft.Json;

public class Program
{
    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals =await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    static async Task<int> getTotalScoredGoals(string team, int year)
    {
        int totalGoals = 0;
        int currentPage = 1;
        int totalPages = 1;

        using (HttpClient client = new HttpClient())
        {
            do
            {
                string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&page={currentPage}";
                HttpResponseMessage response = await client.GetAsync(url);
                string responseContent = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
                totalPages = apiResponse.total_pages;

                foreach (var match in apiResponse.data)
                {
                    if (match.team1 == team)
                    {
                        totalGoals += int.Parse(match.team1goals);
                    }
                    if (match.team2 == team)
                    {
                        totalGoals += int.Parse(match.team2goals);
                    }
                }

                currentPage++;
            } while (currentPage <= totalPages);
        }

        return totalGoals;
    }

    public class ApiResponse
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public List<Match> data { get; set; }
    }

    public class Match
    {
        public string team1 { get; set; }
        public string team2 { get; set; }
        public string team1goals { get; set; }
        public string team2goals { get; set; }
    }

}