using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Epicodemon.Models
{
  public class Mon
  {
    private int _monId;
    private string _monName;
    private int _level;
    private int _hitpoints;
    private int _attack;
    private int _defense;
    private int _specialattack;
    private int _specialdefense;
    private int _speed;

    public Mon(string MonName, int Level, int Hitpoints, int Attack, int Defense, int Specialattack, int Specialdefense, int Speed, int MonId = 0)
    {
      _monId = MonId;
      _monName = MonName;
      _level = Level;
      _hitpoints = Hitpoints;
      _attack = Attack;
      _defense = Defense;
      _specialattack = Specialattack;
      _specialdefense = Specialdefense;
      _speed = Speed;
    }
    public int GetMonId()
    {
      return _monId;
    }
    public string GetMonName()
    {
      return _monName;
    }
    public int GetLevel()
    {
      return _level;
    }
    public int GetHitpoints()
    {
      return _hitpoints;
    }
    public int GetAttack()
    {
      return _attack;
    }
    public int GetDefense()
    {
      return _defense;
    }
    public int GetSpecialattack()
    {
      return _specialattack;
    }
    public int GetSpecialdefense()
    {
      return _specialdefense;
    }
    public int GetSpeed()
    {
      return _speed;
    }
    public override bool Equals(System.Object otherMon)
    {
      if(!(otherMon is Mon))
      {
        return false;
      }
      else
      {
        Mon newMon = (Mon) otherMon;
        bool idEquality = this.GetMonId().Equals(newMon.GetMonId());
        bool nameEquality = this.GetMonName().Equals(newMon.GetMonName());
        bool levelEquality = this.GetLevel().Equals(newMon.GetLevel());
        bool hitpointsEquality = this.GetHitpoints().Equals(newMon.GetHitpoints());
        bool attackEquality = this.GetAttack().Equals(newMon.GetAttack());
        bool defenseEquality = this.GetDefense().Equals(newMon.GetDefense());
        bool spattackEquality = this.GetSpecialattack().Equals(newMon.GetSpecialattack());
        bool spdefenseEquality = this.GetSpecialdefense().Equals(newMon.GetSpecialdefense());
        bool speedEquality = this.GetSpeed().Equals(newMon.GetSpeed());
        return (idEquality && nameEquality && levelEquality && hitpointsEquality && attackEquality && defenseEquality && spattackEquality && spdefenseEquality && speedEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetMonId().GetHashCode();
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO mons (name, level, hitpoints, attack, defense, specialattack, specialdefense, speed) VALUES (@monName, @level, @hitpoints, @attack, @defense, @specialattack, @specialdefense, @speed);";

      cmd.Parameters.Add(new MySqlParameter("@monName", _monName));
      cmd.Parameters.Add(new MySqlParameter("@level", _level));
      cmd.Parameters.Add(new MySqlParameter("@hitpoints", _hitpoints));
      cmd.Parameters.Add(new MySqlParameter("@attack", _attack));
      cmd.Parameters.Add(new MySqlParameter("@defense", _defense));
      cmd.Parameters.Add(new MySqlParameter("@specialattack", _specialattack));
      cmd.Parameters.Add(new MySqlParameter("@specialdefense", _specialdefense));
      cmd.Parameters.Add(new MySqlParameter("@speed", _speed));

      cmd.ExecuteNonQuery();
      _monId = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public static List<Mon> GetAllMons()
    {
      List<Mon> allMons = new List<Mon> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM mons;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int MonId = rdr.GetInt32(0);
        string MonName = rdr.GetString(1);
        int Level = rdr.GetInt32(2);
        int Hitpoints = rdr.GetInt32(3);
        int Attack = rdr.GetInt32(4);
        int Defense = rdr.GetInt32(5);
        int Specialattack = rdr.GetInt32(6);
        int Specialdefense = rdr.GetInt32(7);
        int Speed = rdr.GetInt32(8);

        Mon newMon = new Mon(MonName, Level, Hitpoints, Attack, Defense, Specialattack, Specialdefense, Speed, MonId);
        allMons.Add(newMon);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allMons;
      // return new List<Mon>{};
    }
    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM mons; DELETE FROM mons_battle;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public static Mon Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM mons WHERE id = (@searchId);";

      cmd.Parameters.Add(new MySqlParameter("@searchId", id));

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int MonId = 0;
      string MonName = "";
      int Level = 0;
      int Hitpoints = 0;
      int Attack = 0;
      int Defense = 0;
      int Specialattack = 0;
      int Specialdefense = 0;
      int Speed = 0;
      while(rdr.Read())
      {
        MonId = rdr.GetInt32(0);
        MonName = rdr.GetString(1);
        Level = rdr.GetInt32(2);
        Hitpoints = rdr.GetInt32(3);
        Attack = rdr.GetInt32(4);
        Defense = rdr.GetInt32(5);
        Specialattack = rdr.GetInt32(6);
        Specialdefense = rdr.GetInt32(7);
        Speed = rdr.GetInt32(8);
      }
      Mon newMon = new Mon(MonName, Level, Hitpoints, Attack, Defense, Specialattack, Specialdefense, Speed, MonId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      // return new Mon("", "", 0);
      return newMon;
    }
    public void Edit(string newMonName, int newLevel, int newHitpoints, int newAttack, int newDefense, int newSpecialattack, int newSpecialdefense, int newSpeed)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE mons SET name = @newMonName, hitpoints = @newHitpoints, level = @newLevel, attack = @newAttack, defense = @newDefense, specialattack = @newSpecialattack, specialdefense = @newSpecialdefense, speed = @newSpeed WHERE id = @searchId;";

      cmd.Parameters.Add(new MySqlParameter("@searchId", _monId));
      cmd.Parameters.Add(new MySqlParameter("@newMonName", newMonName));
      cmd.Parameters.Add(new MySqlParameter("@newLevel", newLevel));
      cmd.Parameters.Add(new MySqlParameter("@newHitpoints", newHitpoints));
      cmd.Parameters.Add(new MySqlParameter("@newAttack", newAttack));
      cmd.Parameters.Add(new MySqlParameter("@newDefense", newDefense));
      cmd.Parameters.Add(new MySqlParameter("@newSpecialattack", newSpecialattack));
      cmd.Parameters.Add(new MySqlParameter("@newSpecialdefense", newSpecialdefense));
      cmd.Parameters.Add(new MySqlParameter("@newSpeed", newSpeed));

      cmd.ExecuteNonQuery();
      _monName = newMonName;
      _level = newLevel;
      _hitpoints = newHitpoints;
      _attack = newAttack;
      _defense = newDefense;
      _specialattack = newSpecialattack;
      _specialdefense = newSpecialdefense;
      _speed = newSpeed;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM mons WHERE id = @MonId;";

      cmd.Parameters.Add(new MySqlParameter("@MonId", this.GetMonId()));

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }
    public int GetTrueHP()
    {
      Random iv = new Random();
      // return (((2 * _hitpoints + iv.Next(32)) * _level) / 100) + _level + 10;
      return (((2 * _hitpoints + 31) * _level) / 100) + _level + 10;

    }
    public int GetTrueStat(int stat)
    {
      Random iv = new Random();
      // return (((2 * stat + iv.Next(32)) * _level) / 100) + 5;
      return (((2 * stat + 31) * _level) / 100) + 5;

    }
    public Battle GetAllTrueStats(int id)
    {
      Mon newMon = Mon.Find(id);
      string monName = _monName;
      int trueHP = newMon.GetTrueHP();
      int trueAttack = newMon.GetTrueStat(_attack);
      int trueDefense = newMon.GetTrueStat(_defense);
      int trueSpecialattack = newMon.GetTrueStat(_specialattack);
      int trueSpecialdefense = newMon.GetTrueStat(_specialdefense);
      int trueSpeed = newMon.GetTrueStat(_speed);

      return new Battle(monName, trueHP, trueAttack, trueDefense, trueSpecialattack, trueSpecialdefense, trueSpeed);
    }
  }
}