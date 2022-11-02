using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Social_network.Objects;
using Social_network.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_network
{
    public class Command
    {
        static string ConnectionString
        {
            get
            {
                return new ConfigurationBuilder().AddJsonFile(@"C:\Users\Admin\Desktop\Social-network-master\Social-network-master\Main\appsettings.json").Build().GetConnectionString("SN");
            }
        }
        //private UsersRepository usersRepository = new UsersRepository(ConnectionString);
        //private PostsRepository postsRepository = new PostsRepository(ConnectionString);
        private User currentUser;

        private IMongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<User> _usersCollection;
        private IMongoCollection<Post> _postsCollection;

        public Command()
        {
            _client = new MongoClient(ConnectionString);
            _database = _client.GetDatabase("social-network");
            _usersCollection = _database.GetCollection<User>("users");
            _postsCollection = _database.GetCollection<Post>("posts");
        }

        public bool Authtentificate(string username, string pass)
        {
            var filterBuilder = Builders<User>.Filter;
            var filter = Builders<User>.Filter.Eq("username", username) & filterBuilder.Eq("password", pass);
            var found = _usersCollection.Find(filter).ToList();

            if (found.Count == 0)
            {
                return false;
            }
            currentUser = found[0];
            return true;
        }

        public List<Post> GetStreamPosts()
        {
            var filter = Builders<Post>.Filter.In("username", currentUser.Follows);
            var relatedPosts = _postsCollection.Find(filter).Sort("{date : -1}").ToList();

            return relatedPosts;
        }
        public List<Post> GetStreamPosts(string username)
        {
            var filter = Builders<Post>.Filter.Eq("username", username);
            var relatedPosts = _postsCollection.Find(filter).Sort("{date : -1}").ToList();

            return relatedPosts;
        }

        public List<User> GetFollows()
        {
            var filter = Builders<User>.Filter.In("username", currentUser.Follows);
            var follows =  _usersCollection.Find(filter).ToList();

            return follows;
        }

        public void LikePost(Post post)
        {
            if (!post.Likes.Contains(currentUser.UserName))
            {
                post.Likes.Add(currentUser.UserName);
                Console.WriteLine("You liked this post");

                _postsCollection.ReplaceOne(p => p.Id == post.Id, post);
            }
            else
            {
                post.Likes.Remove(currentUser.UserName);
                Console.WriteLine("You remove your like with this post");

                _postsCollection.ReplaceOne(p => p.Id == post.Id, post);
            }
        }
        public void WriteComment(Post post, string comment)
        {
            post.Comments.Add(new Comment { UserName = currentUser.UserName, CommentText = comment, CreationDate = DateTime.Now });
            _postsCollection.ReplaceOne(p => p.Id == post.Id, post);
        }
        public bool Unfollow(string username)
        {
            bool success = currentUser.Follows.Remove(username);
            if (success)
            {
                _usersCollection.ReplaceOne(u => u.Id == currentUser.Id, currentUser);
            }
            return success;
        }
        public bool CheckUsernameIsFollowed(string username)
        {
            return currentUser.Follows.Contains(username);
        }
        public User FindUser(string username)
        {
            var filter = Builders<User>.Filter.Eq("username", username);
            var users = _usersCollection.Find(filter).ToList();
            
            if (users.Count == 1)
            {
                return users[0];
            }

            return null;
        }
        public bool IsFolllowed(User user)
        {
            return currentUser.Follows.Contains(user.UserName);
        }
        public void Follow(string username)
        {
            currentUser.Follows.Add(username);
            _usersCollection.ReplaceOne(u => u.Id == currentUser.Id, currentUser);
        }

        public void CreateUser(string userName, string firstName, string lastName, string password, List<string> follows)
        {
            var newUser = new User
            {
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                Password = password,
                Follows = follows
            };

            _usersCollection.InsertOne(newUser);
        }

        public void DeleteUser(string userName)
        {
            _usersCollection.DeleteOne(p => p.UserName == userName);
        }
    }
}
