using UnityEngine;
using System.Collections.Generic;
using System;

public class Highscore {

    private SqliteDatabase sql_db = null;

    public List<PlayerScore> BestPlayers
    {
        get
        {
            List<PlayerScore> ret = new List<PlayerScore>();
            if (sql_db != null)
            {
                DataTable dt = sql_db.ExecuteQuery("SELECT * FROM `highscore` ORDER BY `score` DESC, `level` DESC, `name` DESC;");
                foreach (DataRow row in dt.Rows)
                {
                    object name = null;
                    object score = null;
                    object level = null;
                    row.TryGetValue("name", out name);
                    row.TryGetValue("score", out score);
                    row.TryGetValue("level", out level);
                    ret.Add(new PlayerScore(Convert.ToString(name), Convert.ToInt64(score), Convert.ToUInt32(level)));
                }
            }
            return ret;
        }
    }

    public PlayerScore NewScore
    {
        set
        {
            if ((value != null) && (sql_db != null))
                sql_db.ExecuteNonQuery("INSERT INTO `highscore` (`name`, `score`, `level`) VALUES ('" + value.Name.Replace("\"", "\\\"").Replace("\'", "\\\'").Replace("`", "\\`") + "', '" + value.Score + "', '" + value.Level + "');");
        }
    }

    public Highscore(string database)
    {
        sql_db = new SqliteDatabase(database);
        sql_db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS `highscore` (`name` TEXT NOT NULL, `score` INTEGER NOT NULL, `level` INTEGER NOT NULL);");
    }

    public void deleteHighscore()
    {
        if (sql_db != null)
        {
            sql_db.ExecuteNonQuery("DELETE FROM `highscore`;");
            sql_db.ExecuteNonQuery("UPDATE `sqlite_sequence` SET `seq`='0' WHERE `name`='highscore';");
            sql_db.ExecuteNonQuery("VACUUM;");
        }
    }

    public void dropHighscore()
    {
        if (sql_db != null)
        {
            sql_db.ExecuteNonQuery("DROP TABLE IF EXISTS `highscore`;");
            sql_db.ExecuteNonQuery("DELETE FROM `sqlite_sequence` WHERE `name`='highscore';");
            sql_db.ExecuteNonQuery("VACUUM;");
        }
    }
}
