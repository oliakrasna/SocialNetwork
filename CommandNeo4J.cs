using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neo4J
{
    public class CommandNeo4J
    {
        static BoltGraphClient Client
        {
            get
            {
                BoltGraphClient client = new BoltGraphClient("neo4j+s://28503b13.databases.neo4j.io:3789", "neo4j", "_ayfnYRTV56WExe_e23-Qr-5gSDOx6JRKXyr6");
                client.ConnectAsync().Wait();
                return client;
            }
        }
        private User currentUser;
        public void Authtentificate(string username, string pass)
        {
            var user = Client.Cypher
                .Match("(u:User { username: $un})")
                .WithParam("un", username)
                .Where("u.password= $pass")
                .WithParam("pass", pass)
                .Return(u => u.As<User>())
                .ResultsAsync.Result;
            currentUser = user.ElementAt(0);
        }
        public void CreateUser(string userName, string firstName, string surname, string password)
        {
            var newUser = new User
                (
                userName,
                firstName,
                surname,
                password
                );
            Client.Cypher
                .Create("(u:User $newUser)")
                .WithParam("newUser", newUser)
                .ExecuteWithoutResultsAsync().Wait();

        }
        public void CreateRelationshipUserFollow(string SubscribedName)
        {
            Client.Cypher
                .Match("(u:User{username:$un})", "(f:User{username: $fn})")
                .WithParam("un", currentUser.UserName)
                .WithParam("fn", SubscribedName)
                .Create("(u)-[:Subscribed]->(f)")
                .ExecuteWithoutResultsAsync().Wait();
        }
        public void DeleteRelationshipUserFollow(string SubscribedName)
        {
            Client.Cypher
                .Match("(u:User{username:$un})-[r:Subscribed]->(f:User{username: $fn})")
                .WithParam("un", currentUser.UserName)
                .WithParam("fn", SubscribedName)
                .Delete("r")
                .ExecuteWithoutResultsAsync().Wait();
        }
        public void DeleteUser(string userName)
        {
            DeleteAllRelationshipWithUser(userName);
            Client.Cypher
                .Match("(u:User {username: $deleteUser})")
                .WithParam("deleteUser", userName)
                .Delete("u")
                .ExecuteWithoutResultsAsync().Wait();

        }
        public void DeleteAllRelationshipWithUser(string userName)
        {
            Client.Cypher
                .Match("(u:User{username:$un})-[r]-(f:User)")
                .WithParam("un", currentUser.UserName)
                .Delete("r")
                .ExecuteWithoutResultsAsync().Wait();
        }
        public IEnumerable<Object> SearchRelationshipOfUser(string searchedUser)
        {
            var userWithSubscribed = Client.Cypher
                .Match("(u:User {username: $un})-[r]-> (f: User {username: $fn})")
                .WithParam("un", currentUser.UserName)
                .WithParam("fn", searchedUser)
                .Return((u, f) => new
                {
                    User = u.As<User>(),
                    Follower = f.As<User>()
                })
                .ResultsAsync.Result;
            return userWithSubscribed;
        }
        public double ShortestPathToSearchedUser(string searchedUserName)
        {
            var userWithSubscribed = Client.Cypher
                .Match("sp = shortestPath((:User {username: $un})-[*]-(:User {username: $fn}))")
                .WithParam("un", currentUser.UserName)
                .WithParam("fn", searchedUserName)
                .Return(sp => sp.Length())
                .ResultsAsync.Result;
            return userWithSubscribed.First();
        }
    }
}

