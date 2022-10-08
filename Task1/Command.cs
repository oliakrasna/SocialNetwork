using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{

    public class Command
    {
        static string ConnectionString
        {
            get
            {
                return new ConfigurationBuilder().AddJsonFile(@"C:\Users\Admin\source\repos\Task1\Task1\ConnectionString.json").Build().GetConnectionString("SN");
            }
        }

        private User presentUser;

        private IMongoClient customer;
        private IMongoDatabase data_base;
        private IMongoCollection<User> users_asemblage;
        private IMongoCollection<Post> posts_asemblage;

        public void AlikeThePost(Post post)
        {
            if (!post.Likes.Contains(presentUser.UserName))
            {
                post.Likes.Add(presentUser.UserName);
                Console.WriteLine("You liked post");

                posts_asemblage.ReplaceOne(p => p.Id == post.Id, post);
            }
            else
            {
                post.Likes.Remove(presentUser.UserName);
                Console.WriteLine("You stop this post");

                posts_asemblage.ReplaceOne(p => p.Id == post.Id, post);
            }
        }
       
        public void Follow(string username)
        {
            presentUser.Follows.Add(username);
            users_asemblage.ReplaceOne(u => u.Id == presentUser.Id, presentUser);
        }
        public List<Post> GetFlowOfPosts()
        {
            var strainer = Builders<Post>.Filter.In("username", presentUser.Follows);
            var agned_posts = posts_asemblage.Find(strainer).Sort("{date : -1}").ToList();

            return agned_posts;
        }
        public List<Post> GetFlowPosts(string username)
        {
            var strainer = Builders<Post>.Filter.Eq("username", username);
            var agned_posts = posts_asemblage.Find(strainer).Sort("{date : -1}").ToList();

            return agned_posts;
        }

        public void PrintTheComment(Post post, string comment)
        {
            post.Comments.Add(new Comment { UserName = presentUser.UserName, CommentText = comment, CreationDate = DateTime.Now });
            posts_asemblage.ReplaceOne(p => p.Id == post.Id, post);
        }
        public bool StopFollow(string username)
        {
            bool success = presentUser.Follows.Remove(username);
            if (success)
            {
                users_asemblage.ReplaceOne(u => u.Id == presentUser.Id, presentUser);
            }
            return success;
        }
        public bool LookIfUsernameIsFollowed(string username)
        {
            return presentUser.Follows.Contains(username);
        }
        public User LookForUser(string username)
        {
            var strainer = Builders<User>.Filter.Eq("username", username);
            var users = users_asemblage.Find(strainer).ToList();

            if (users.Count == 1)
            {
                return users[0];
            }

            return null;
        }
        public List<User> GetPeopleIFollow()
        {
            var strainer = Builders<User>.Filter.In("username", presentUser.Follows);
            var follows = users_asemblage.Find(strainer).ToList();

            return follows;
        }
        public bool IsFolllowed(User user)
        {
            return presentUser.Follows.Contains(user.UserName);
        }

        public bool Authtentification(string username, string pass)
        {
            var filterBuilder = Builders<User>.Filter;
            var filter = Builders<User>.Filter.Eq("username", username) & filterBuilder.Eq("password", pass);
            var found = users_asemblage.Find(filter).ToList();

            if (found.Count == 0)
            {
                return false;
            }
            presentUser = found[0];
            return true;
        }
        public Command()
        {
            customer = new MongoClient(ConnectionString);
            data_base = customer.GetDatabase("SocialNetwork");
            users_asemblage = data_base.GetCollection<User>("users");
            posts_asemblage = data_base.GetCollection<Post>("posts");
        }

    }
}
