using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System;

public class ASM_MN : Singleton<ASM_MN>
{
    public List<Region> listRegion = new List<Region>();
    public List<Players> listPlayer = new List<Players>();


    private void Start()
    {
        createRegion();
    }

    public void createRegion()
    {
        listRegion.Add(new Region(0, "VN"));
        listRegion.Add(new Region(1, "VN1"));
        listRegion.Add(new Region(2, "VN2"));
        listRegion.Add(new Region(3, "JS"));
        listRegion.Add(new Region(4, "VS"));
    }

    public string calculate_rank(int score)
    {
        // Tính toán xếp hạng dựa trên điểm số
        if (score < 100)
            return "Đồng";
        else if (score < 500)
            return "Bạc";
        else if (score < 1000)
            return "Vàng";
        else
            return "Kim cương";
    }


    public void YC1()
    {
        // Lấy các giá trị từ ScoreKeeper
        string name = ScoreKeeper.Instance.GetUserName();
        int id = ScoreKeeper.Instance.GetID();
        int idR = ScoreKeeper.Instance.GetIDregion();
        int score = ScoreKeeper.Instance.GetScore();

        // Lấy tên vùng dựa theo id
        string regionName = listRegion.FirstOrDefault(r => r.ID == idR)?.Name ?? "Unknown";

        // Thêm thông tin người chơi mới khi nhập từ text
        Region playerRegion1 = new Region(idR, regionName);
        Players player3 = new Players(id, name, score, playerRegion1);
        listPlayer.Add(player3);

        // Thêm người chơi giả lập để kiểm tra
        Players player1 = new Players(id, "Nam", 5, new Region(2, "VN2"));
        listPlayer.Add(player1);
        Players player2 = new Players(id, "Huy", 100000, new Region(3, "JS"));
        listPlayer.Add(player2);
    }

    public void YC2()
    {

        // Duyệt và in các thông tin của từng đối tượng trong Players ra
        foreach (Players player in listPlayer)
        {
            string rank = calculate_rank(player.Score);
            Debug.Log("Player Name: " + player.Name + " - Score: " + player.Score + " - Region: " + player.PlayerRegion.Name+"-Rank:"+rank);
        }
    }
    public void YC3()
    {
        if (listPlayer.Count == 0)
        {
            Debug.Log("không có người chơi nào khác.");
            return;
        }

        int currentPlayerScore = listPlayer[0].Score;
        var less = listPlayer.Where(Pr => Pr.Score < currentPlayerScore);

        if (!less.Any())
        {
            Debug.Log("không có người chơi nào khác tệ hơn bạn.");
            return;
        }

        Debug.Log("Player có score bé hơn score hiện tại của người chơi.");
        foreach (var player in less)
        {
            string rank = calculate_rank(player.Score);
            Debug.Log("Name: " + player.Name + " - score: " + player.Score + " - Rank: " + rank);
        }
    }
    public void YC4()
    {
        int currentPlayerId = listPlayer[0].Id; // Giả sử người chơi hiện tại có Id là Id của người chơi đầu tiên trong danh sách

        // Sử dụng LINQ để tìm người chơi theo Id
        var findPlayer = listPlayer.FirstOrDefault(player => player.Id == currentPlayerId);

        if (findPlayer != null)
        {
            string rank = calculate_rank(findPlayer.Score);
            Debug.Log("Thông tin của người chơi có Id " + currentPlayerId + ":");
            Debug.Log("Name: " + findPlayer.Name + " - Score: " + findPlayer.Score + " - Region: " + findPlayer.PlayerRegion.Name + " - Rank: " + rank);
        }
        else
        {
            Debug.Log("Không tìm thấy người chơi có Id " + currentPlayerId);
        }
    }
    public void YC5()
    {
        // Sắp xếp danh sách người chơi theo điểm số giảm dần
        var sortedPlayers = listPlayer.OrderByDescending(player => player.Score);

        // Xuất thông tin các người chơi sau khi sắp xếp
        Debug.Log("Thông tin các người chơi theo thứ tự điểm số giảm dần:");
        foreach (var player in sortedPlayers)
        {
            string rank = calculate_rank(player.Score);
            Debug.Log("Name: " + player.Name + " - Score: " + player.Score + " - Region: " + player.PlayerRegion.Name + " - Rank: " + rank);
        }
    }

    public void YC6()
    {
        // Sắp xếp danh sách người chơi theo điểm số tăng dần
        var sortedPlayers = listPlayer.OrderBy(player => player.Score);

        // Lấy 5 người chơi có điểm số thấp nhất
        var lowestScorePlayers = sortedPlayers.Take(5);

        // Xuất thông tin của 5 người chơi có điểm số thấp nhất
        Debug.Log("Thông tin 5 người chơi có điểm số thấp nhất:");
        foreach (var player in lowestScorePlayers)
        {
            string rank = calculate_rank(player.Score);
            Debug.Log("Name: " + player.Name + " - Score: " + player.Score + " - Region: " + player.PlayerRegion.Name + " - Rank: " + rank);
        }
    }
    public void YC7()
    {
        // Tạo và bắt đầu một luồng (Thread) có tên "BXH"
        Thread BXH = new Thread(() =>
        {
            // Tạo một Dictionary để lưu trữ điểm trung bình của từng khu vực (Region)
            Dictionary<string, double> averageScoresByRegion = new Dictionary<string, double>();

            // Duyệt qua danh sách người chơi và tính điểm trung bình cho từng khu vực
            foreach (var player in listPlayer)
            {
                if (!averageScoresByRegion.ContainsKey(player.PlayerRegion.Name))
                {
                    // Lọc danh sách người chơi theo khu vực và tính điểm trung bình
                    var playersInRegion = listPlayer.Where(p => p.PlayerRegion.Name == player.PlayerRegion.Name);
                    double averageScore = playersInRegion.Average(p => p.Score);
                    averageScoresByRegion.Add(player.PlayerRegion.Name, averageScore);
                }
            }

            // Ghi điểm trung bình của từng khu vực vào tập tin bxhReigon.txt
            using (StreamWriter writer = new StreamWriter("bxhReigon.txt"))
            {
                foreach (var entry in averageScoresByRegion)
                {
                    writer.WriteLine("Region: " + entry.Key + " - Average Score: " + entry.Value);
                }
            }

            Debug.Log("Tính toán và lưu điểm trung bình theo khu vực hoàn thành.");
        });

        BXH.Start(); // Khởi động luồng "BXH"
    }
    void CalculateAndSaveAverageScoreByRegion()
    {
        // sinh viên viết tiếp code ở đây
    }

}

[SerializeField]
public class Region
{
    public int ID;
    public string Name;
    public Region(int ID, string Name)
    {
        this.ID = ID;
        this.Name = Name;
    }
}

[SerializeField]
public class Players
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Score { get; set; }
    public Region PlayerRegion { get; set; }

    // Constructor
    public Players(int id, string name, int score, Region region)
    {
        Id = id;
        Name = name;
        Score = score;
        PlayerRegion = region;
    }
}