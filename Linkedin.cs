using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LinkedIn
{
    public enum StudyPlace
    {
        BenGurion,
        TelAviv
    }
    public enum Field
    {
        Physics,
        CS
    }
    public class Education
    {
        public StudyPlace StudyPlace { get; set; }
        public Field Field { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public Education(StudyPlace studyPlace, Field field, DateTime start, DateTime end)
        {
            StudyPlace = studyPlace;
            Field = field;
            Start = start;
            End = end;
        }
    }
    public enum Universities
    {
        TelAviv,
        Technion,
        BarIlan,
        BenGurion
    }
    public enum Fields
    {
        Math,
        ComputerScience,
        Physics,
        DoubleDegreeMathAndCS
    }
    public class StudyField
    {
        public Universities University { get; private set; }
        public Fields Field { get; private set; }
        public double Years { get; private set; }

        public StudyField(Universities university, Fields field, double years)
        {
            University = university;
            Field = field;
            Years = years;
        }
    }
    public class Picture
    {
        public string PictureLink { get; private set; }

        public Picture(string pictureLink)
        {
            PictureLink = pictureLink;
        }
    }
    public class Post
    {
        public Picture Picture { get; private set; }
        public string Text { get; private set; }
        public int Likes { get; private set; } 
        public List<string> Comments { get; private set; }
        public Post(Picture picture, string text, int likes, List<string> comments)
        {
            Picture = picture;
            Text = text;
            Likes = likes;
            Comments = comments;
        }

        public void Like()
        {
            Likes++;
        }
        public void Comment(string Comment)
        {
            Comments.Add(Comment);
        }
    }
    public class Profile
    {
        private string _bio;
        private List<Post> _posts;
        public string Bio
        {
            get { return _bio; }
            set { _bio = value; }
        }
        public List<Post> Post
        {
            get { return _posts; }
            set { _posts = value; }
        }
    }
    public enum Profession
    {
        SoftwareEngieneer,
        QA,
        DevOps,
        Manager
    }
    public abstract class User
    {
        private string _userName;
        private string _iD;
        private string _email;
        private int _age;
        private Profession _profession;
        private string _creditCard;
        private List<Website> _websites;
        private List<Education> _educations;
        private bool _isPremium;

        #region C'tor

        #endregion
        #region Getters&Setters
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        public string ID
        {
            get { return _iD; }
            set { _iD = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public int Age
        {
            get { return _age; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");
                _age = value;
            }
        }
        public Profession Profession
        {
            get { return _profession; }
            set
            {
                if (value != Profession)
                    throw new ArgumentException("Profession must be in category"); //!!!
                _profession = value;
            }
        }
        public string CreditCard
        {
            get { return _creditCard; }
            set { _creditCard = value; }
        }
        public bool IsPremium
        {
            get { return _isPremium; }
            set { _isPremium = value; }
        }
        #endregion
    }
    public class Website
    {
        public string Link { get; private set; }
        public string URL { get; private set; }

        public Website(string link, string url)
        {
            Link = link;
            URL = url;
        }
    }
    public class WorkPlace
    {
        public string Name { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public WorkPlace(string name, DateTime start, DateTime end)
        {
            Name = name;
            Start = start;
            End = end;
        }
    }
    internal class Linkedin
    {
        static void Main(string[] args)
        {
        }
    }
}
