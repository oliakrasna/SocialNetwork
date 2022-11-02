using Social_network.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using Neo4J;

namespace Social_network
{
    public class Menu
    {
		private Command command = new Command();
		private CommandNeo4J command2 = new CommandNeo4J();

		public void showMenu()
		{
			char user_input;
			Console.Write("Log in? (y/n): ");
			user_input = Console.ReadLine()[0];
			if (user_input != 'y' && user_input == 'n') { Console.Write("Wrong letter typed"); }
			else { Authentication(user_input); showMainMenu(); }
		}

		public void Authentication(char userInput)
		{
			bool authenticationRes;
			while (userInput == 'y')
			{
				Console.Clear();
				authenticationRes = doAuthentication();
				if (authenticationRes)
				{
					break;
				}
				else
				{
					Console.Write("\nTyped worng password or username, try again? (y/n): ");
					userInput = Console.ReadKey().KeyChar;
				}
			}
		}
		private bool doAuthentication()
        {
			Console.Write("Enter username: ");
			string username = Console.ReadLine();
			
			Console.Write("Enter password: ");
			var pass = string.Empty;
			ConsoleKey key;
			do
			{
				var keyInfo = Console.ReadKey(intercept: true);
				key = keyInfo.Key;

				if (key == ConsoleKey.Backspace && pass.Length > 0)
				{
					Console.Write("\b \b");
					pass = pass.Substring(0, pass.Length - 1);
				}
				else if (!char.IsControl(keyInfo.KeyChar))
				{
					Console.Write("*");
					pass += keyInfo.KeyChar;
				}
			} while (key != ConsoleKey.Enter);
			var successAuthentication = command.Authtentificate(username, pass);
			command2.Authtentificate(username, pass);
			return command.Authtentificate(username, pass);
		}
		private void showMainMenu()
		{
			char userInput;

			do
			{
				Console.Clear();
				Console.WriteLine("\nMain menu\n");
				Console.WriteLine("\nPlease select an option:\n");
				Console.WriteLine("1 - Posts stream");
				Console.WriteLine("2 - My follows");
				Console.WriteLine("3 - Search");
				Console.WriteLine("4 - Create user");
				Console.WriteLine("5 - Remove user");
				Console.WriteLine("0 - Exit");
				Console.Write("Your choice >> ");

				userInput = Console.ReadLine()[0];
				safeHandleMainMenuInput(userInput);

			} while (userInput != '0');
		}
		private void handleMainMenuInput(char userInput)
		{
			switch (userInput)
			{
				case '1':
					
					showStreamMenu();
					break;
				case '2':
					
					showFollowsMenu();
					break;
				case '3':
					
					showSearchMenu();
					break;
				case '4':

					showCreateMenu();
					break;
				case '5':

					showRemoveMenu();
					break;
				case '0':
					break;
				default:
					Console.WriteLine("Wrong command selected");
					break;
			}
		}

		private void showCreateMenu()
		{
			Console.Clear();
			string userName;
			Console.Write("Type new username : ");
			userName = Console.ReadLine();
			string firstName;
			Console.Write("Type new first name : ");
			firstName = Console.ReadLine();
			string lastName;
			Console.Write("Type new last name : ");
			lastName = Console.ReadLine();
			string password;
			Console.Write("Type new password : ");
			password = Console.ReadLine();
			List<string> follows = new List<string>();

			command.CreateUser(userName, firstName, lastName, password, follows);
			command2.CreateUser(userName, firstName, lastName, password);
		}
		private void showRemoveMenu()
		{
			Console.Clear();
			string userName;
			Console.Write("Type the username for removing : ");
			userName = Console.ReadLine();
			command.DeleteUser(userName);
			
			command2.DeleteUser(userName);
		}

		private void safeHandleMainMenuInput(char userInput)
		{
			try
			{
				handleMainMenuInput(userInput);
			}
			catch (Exception ex)
			{
				

				Console.WriteLine($"Error occurs: {ex.Message}");
			}
		}

		private void showStreamMenu()
        {
			char userInput;
			Console.Clear();

			var posts = command.GetStreamPosts();
			int index = 0;
			bool next_post = true;

			do
			{
				if (next_post)
                {
					Console.Clear();
					Console.WriteLine(posts[index]);
					Console.WriteLine("1 - Like    2 - Comments    3 - Next post    0 - Exit");
				}
				Console.Write("Your choice >> ");

				userInput = Console.ReadLine()[0];
				safeHandleStreamMenuInput(userInput, posts[index], ref index, ref next_post);

			} while (userInput != '0' && index < posts.Count);
			
			if (index == posts.Count)
            {
				Console.WriteLine("\nThat was last post for now\n Press any button to back in main menu...");
				Console.ReadLine();
            }
		}

		private void safeHandleStreamMenuInput(char userInput, Post post, ref int index, ref bool next_post)
		{
			try
			{
				handleStreamMenuInput(userInput, post, ref index, ref next_post);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error occurs: {ex.Message}");
			}
		}
		private void handleStreamMenuInput(char userInput, Post post, ref int index, ref bool next_post)
		{
			switch (userInput)
			{
				case '1':
				
					command.LikePost(post);
					next_post = false;
					break;
				case '2':
					
					showCommentsMenu(post);
					next_post=true;
					break;
				case '3':
				
					index++;
					next_post = true;
					break;
				case '0':
					break;
				default:
					Console.WriteLine("Wrong command selected");
					break;
			}
		}
		private void showCommentsMenu(Post post)
        {
			char userInput;

			do
			{
				Console.Clear();
				foreach (Comment comment in post.Comments.OrderBy(c => c.CreationDate))
				{
					Console.WriteLine(comment);
					Console.WriteLine("================================================================");
				}
				Console.WriteLine("1 - Write comment    0 - Exit");
				Console.Write("Your choice >> ");

				userInput = Console.ReadLine()[0];
				safeHandleCommentsMenuInput(post, userInput);

			} while (userInput != '0');

		}
		private void safeHandleCommentsMenuInput(Post post, char userInput)
        {
			try
			{
				handleCommentsMenuInput(post, userInput);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error occurs: {ex.Message}");
			}
		}
		private void handleCommentsMenuInput(Post post, char userInput)
        {
			string userComment;

            switch (userInput)
			{
				case '1':
					Console.Write("Your comment >> ");
					userComment = Console.ReadLine();
					command.WriteComment(post, userComment);
					break;
				case '0':
					break;
				default:
					Console.WriteLine("Wrong command selected");
					break;
			}
		}
		private void showFollowsMenu()
		{
			char userInput;
			Console.Clear();

			List<Objects.User> follows;
			do
			{
				follows = command.GetFollows();
				Console.WriteLine("\nMy Follows:\n");
				foreach(var f in follows)
                {
					Console.WriteLine(f);
                }

				Console.WriteLine("1 - Unfollow    2 - Posts    0 - Exit");
				Console.Write("Your choice >> ");

				userInput = Console.ReadLine()[0];
				safeHandleFollowsMenuInput(userInput);

			} while (userInput != '0');
		}

        private void safeHandleFollowsMenuInput(char userInput)
        {
			try
			{
				handleFollowsMenuInput(userInput);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error occurs: {ex.Message}");
			}
		}

        private void handleFollowsMenuInput(char userInput)
        {
			switch (userInput)
            {
				case '1':
					unFollowUserMenuCommand();
					break;
				case '2':
					showFollowStream();
					break;
				case '0':
					break;
				default:
					Console.WriteLine("Wrong command selected");
					break;
			}
        }
		
		private void showFollowStream()
        {
			string selected;

			Console.Write("Write username >> ");
			selected = Console.ReadLine();

			if (command.CheckUsernameIsFollowed(selected))
            {
				var posts = command.GetStreamPosts(selected);

				if (posts.Count == 0)
                {
					Console.WriteLine("This user does not have any post yet :(");
					return;
                }

				int index = 0;
				char userInput;
				bool next_post = true;

				do
				{
					if (next_post)
					{
						Console.Clear();
						Console.WriteLine(posts[index]);
						Console.WriteLine("1 - Like    2 - Comments    3 - Next post    0 - Exit");
					}
					Console.Write("Your choice >> ");

					userInput = Console.ReadLine()[0];
					safeHandleStreamMenuInput(userInput, posts[index], ref index, ref next_post);

				} while (userInput != '0' && index < posts.Count);

				if (index == posts.Count)
				{
					Console.WriteLine("\nThat was last post for now\n Press any button to back in Follows menu...");
					Console.ReadLine();
				}
			}
            else
            {
				Console.WriteLine("Error! Wrong username.");
			}
		}

		private void showSearchMenu()
        {
			char userInput;
			Console.Clear();

			do
			{
				Console.WriteLine("1 - Search    0 - Exit");
				Console.Write("Your choice >> ");

				userInput = Console.ReadLine()[0];
				safeHandleSearchMenuInput(userInput, Neo4J.User user);

			} while (userInput != '0');
		}

        private void safeHandleSearchMenuInput(char userInput, Neo4J.User user)
        {
			try
			{
				handleSearchMenuInput(userInput, Neo4J.User user);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error occurs: {ex.Message}");
			}
		}

        private void handleSearchMenuInput(char userInput, Neo4J.User user)
        {
            switch (userInput)
            {
				case '1':
					searchUser(user);
					break;
				case '2':
					userFolloweOptionMenu(user);
					break;
				case '0':
					break;
				default:
					Console.WriteLine("Wrong command selected");
					break;
			}
        }

        private void searchUser(Neo4J.User user)
        {
			string username;

			Console.Write("User Search\nEnter username >>");
			username = Console.ReadLine();

			var foundUser = command.FindUser(username);


			if (foundUser != null)
            {
				showUserMenu(foundUser);
            }
            else
            {
				Console.WriteLine("Wrong username! This user does not exist");
            }
        }

        private void showUserMenu(Neo4J.User user)
        {
			char userInput;
			Console.Clear();

			do
			{
				Console.WriteLine("Profile:");
				Console.WriteLine(user);
				ExistRelationshipMenu(user.UserName);
				if (command.IsFolllowed(user))
                {
					Console.WriteLine("You follow this user.");
                }
                else
                {
					Console.WriteLine("You not follow this user.");
				}

				Console.WriteLine("1 - Posts    2 - Follow/Unfollow    0 - Exit");
				Console.Write("Your choice >> ");

				userInput = Console.ReadLine()[0];
				safeHandleUserMenuInput(userInput, user);

			} while (userInput != '0');
		}


		private void ExistRelationshipMenu(string username)
		{
			var existRelationship = command2.SearchRelationshipOfUser(username);

			if (existRelationship.Count() != 0)
			{
				Console.WriteLine("\nYou have relationship with this user )");
				Console.WriteLine($"The distance to this user : {command2.ShortestPathToSearchedUser(username)}");
			}
			else
			{
				Console.WriteLine("\nYou haven`t relationship with this user (");
				Console.WriteLine($"The distance to this user : {command2.ShortestPathToSearchedUser(username)}");
			}
		}
		private void safeHandleUserMenuInput(char userInput, Neo4J.User user)
        {
			try
			{
				handleUserMenuInput(userInput, user);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error occurs: {ex.Message}");
			}
		}
		private void userFolloweOptionMenu(User user)
		{

			if (command.CheckUsernameIsFollowed(user.UserName) == true)
			{
				command.Unfollow(user.UserName);
				command2.DeleteRelationshipUserFollow(user.UserName);
				Console.WriteLine($"\nYou not follow user : {user.UserName}");
			}
			else
			{
				command.Follow(user.UserName);
				command2.CreateRelationshipUserFollow(user.UserName);
				Console.WriteLine($"\nYou follow user  : {user.UserName}");
			}

		}

		private void unFollowUserMenuCommand()
		{
			string selected;
			bool? success;
			Console.Write("\nWrite username : ");
			selected = Console.ReadLine();
			success = command.Unfollow(selected);
			command2.DeleteRelationshipUserFollow(selected);
			if (success == true)
			{
				Console.WriteLine($"Unfollowed user: {selected}");
			}
		}
		private void handleUserMenuInput(char userInput, User user)
        {
            switch (userInput)
            {
				case '1':
					showUserPostsStream(user);
					break;
				case '2':
					if (command.IsFolllowed(user))
					{
						command.Unfollow(user.UserName);
						Console.WriteLine($"Now you not follow user {user.UserName}");
					}
                    else
                    {
						command.Follow(user.UserName);
						Console.WriteLine($"Now you follow user {user.UserName}");
					}
					break;
				case '0':
					break;
				default:
					Console.WriteLine("Wrong commnand selected");
					break;
            }
        }

        private void showUserPostsStream(User user)
        {
			char userInput;
			Console.Clear();

			var posts = command.GetStreamPosts(user.UserName);

			if (posts.Count == 0)
            {
				Console.WriteLine("This user does not have any post yet :(");
				return;
            }

			int index = 0;
			bool next_post = true;

			do
			{
				if (next_post)
				{
					Console.Clear();
					Console.WriteLine(posts[index]);
					Console.WriteLine("1 - Like    2 - Comments    3 - Next post    0 - Exit");
				}
				Console.Write("Your choice >> ");

				userInput = Console.ReadLine()[0];
				safeHandleStreamMenuInput(userInput, posts[index], ref index, ref next_post);

			} while (userInput != '0' && index < posts.Count);

			if (index == posts.Count)
			{
				Console.WriteLine("\nThat was last post for now\n Press any button to back in follow menu...");
				Console.ReadLine();
			}
		}
    }
}
