using UnityEngine;
using System;

public class Role
{
    public int id ;
    public string name ;
    public string description ;
    public string type ;
}

public class Project
{
    public int id ;
    public string name ;
    public DateTime published_at ;
    public DateTime created_at ;
    public DateTime updated_at ;
    public int users_permissions_user ;
    public int owner ;
}

public class User
{
    public int id ;
    public string username ;
    public string email ;
    public string provider ;
    public bool confirmed ;
    public bool blocked ;
    public Role role ;
    public DateTime created_at ;
    public DateTime updated_at ;
    public Project project ;
}

public class LoginData
{
    public string jwt ;
    public User user ;

    public static LoginData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<LoginData>(jsonString);
    }
}

