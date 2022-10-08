using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    
		public class Menu
		{
			private Command command = new Command();

			public void lookMenu()
			{
				char userInput = 'Y';
				bool authRes;
			while (userInput == 'Y') 
			{
					Console.Clear();
					authRes = operationAuthentication();
					if (authRes)
					{
						displayBasicMenu();
					}
					else
					{
						Console.Write("\nYou wrote worng password or username, try again: ");
						userInput = Console.ReadLine()[0];
					}
				} 
			}
			private bool operationAuthentication()
			{
				Console.Write("Write username: ");
				string username = Console.ReadLine();

				Console.Write("Write password: ");
				string password = Console.ReadLine();

			return command.Authtentification(username, password);
			}
		private void handleMainMenuInput(char userInput)
		{
			switch (userInput)
			{
				case '1':
					showFlowMenu();
					break;
				case '2':
					displayFollowsMenu();
					break;
				case '3':
					showLookingForMenu();
					break;
				case '0':
					break;
				default:
					Console.WriteLine("Error, choose right option!");
					break;
			}
		}
		private void displayFollowsMenu()
		{
			char userInput= ' ' ;
			Console.Clear();

			List<User> follows;
			while (userInput != '0') 
			{
				follows = command.GetPeopleIFollow();
				Console.WriteLine("\nMy Follows:\n");
				foreach (var f in follows)
				{
					Console.WriteLine(f);
				}

				Console.WriteLine("1. Unfollow person    2. Posts    0. Exit");
				Console.Write("You select  ");

				userInput = Console.ReadLine()[0];

			} 
		}

		private void displayBasicMenu()
			{
				char userInput = ' ';

			while (userInput != '0') 
			{
					Console.Clear();
					Console.WriteLine("\nBasic menu\n");
					Console.WriteLine("\nChoose preference:\n");
					Console.WriteLine("1. Astream of posts");
					Console.WriteLine("2. Follows");
					Console.WriteLine("3. Looking for");
					Console.WriteLine("0. Exit");
					Console.Write("You select  ");

					userInput = Console.ReadLine()[0];
					handleMainMenuInput(userInput);

				} 
			}
		private void displayCommentsMenu(Post post)
		{
			char userInput = ' ';

			while (userInput != '0')
			{
				Console.Clear();
				foreach (Comment comment in post.Comments.OrderBy(c => c.CreationDate))
				{
					Console.WriteLine(comment);
				}
				Console.WriteLine("1. Print the comment    0. Exit");
				Console.Write("You select ");

				userInput = Console.ReadLine()[0];
				handleCommentsMenuInput(post, userInput);

			} 

		}
		private void displayUserMenuForFollowers(User user)
		{
			char userInput = ' ';
			Console.Clear();

			while (userInput != '0') 
			{
				Console.WriteLine("Page:");
				Console.WriteLine(user);

				if (command.IsFolllowed(user))
				{
					Console.WriteLine("The user you followed.");
				}
				else
				{
					Console.WriteLine("The user you DON'T follow.");
				}

				Console.WriteLine("1. Flow of posts    2. Follow OR Unfollow    0.Exit");
				Console.Write("You select  ");

				userInput = Console.ReadLine()[0];
				handleUserMenuInput(userInput, user);

			} 
		}

			
			private void handleStreamMenuInput(char userInput, Post post, ref int indx, ref bool nearby_post)
			{
				switch (userInput)
				{
					case '1':
						command.AlikeThePost(post);
						nearby_post = false;
						break;
					case '2':
						displayCommentsMenu(post);
						nearby_post = true;
						break;
					case '3':
						indx++;
						nearby_post = true;
						break;
					case '0':
						break;
					default:
						Console.WriteLine("You choose wrong option!");
						break;
				}
			}
			
			
			private void handleCommentsMenuInput(Post post, char userInput)
			{
				string userComment;

				switch (userInput)
				{
					case '1':
						Console.Write("Comment you wrote ");
						userComment = Console.ReadLine();
						command.PrintTheComment(post, userComment);
						break;
					case '0':
						break;
					default:
						Console.WriteLine("You choose wrong option!");
						break;
				}
			}

			private void handleLookingForMenuInput(char userInput)
			{
				switch (userInput)
				{
					case '1':
						lookingforUser();
						break;
					case '0':
						break;
					default:
						Console.WriteLine("You choose wrong option!");
						break;
				}
			}

		private void handleFollowsMenuInput(char userInput)
			{
				switch (userInput)
				{
					case '1':
						stoppedfollowUser();
						break;
					case '2':
						showFollowFlow();
						break;
					case '0':
						break;
					default:
						Console.WriteLine("You choose wrong option!");
						break;
				}
			}
		private void handleUserMenuInput(char userInput, User user)
		{
			switch (userInput)
			{
				case '1':
					showUserPostsFlow(user);
					break;
				case '2':
					if (command.IsFolllowed(user))
					{
						command.StopFollow(user.UserName);
						Console.WriteLine($"You stop follow this user {user.UserName}");
					}
					else
					{
						command.Follow(user.UserName);
						Console.WriteLine($"You start follow user {user.UserName}");
					}
					break;
				case '0':
					break;
				default:
					Console.WriteLine("You choose wrong username!");
					break;
			}
		}


		private void stoppedfollowUser()
			{
				string selected;
				bool success;

				Console.Write("Print the username ");
				selected = Console.ReadLine();
				success = command.StopFollow(selected);

				if (success)
				{
					Console.WriteLine($"My followed user: {selected}");
				}
				else
				{
					Console.WriteLine("You choose wrong username!");
				}
			}

			private void lookingforUser()
			{
				string username;

				Console.Write("Looking for a user\nPrint username >>");
				username = Console.ReadLine();

				var foundUser = command.LookForUser(username);


				if (foundUser != null)
				{
					displayUserMenuForFollowers(foundUser);
				}
				else
				{
					Console.WriteLine("You choose wrong username!");
				}
			}



		private void showFlowMenu()
		{
			char userInput = ' ';
			Console.Clear();

			var posts = command.GetFlowOfPosts();
			int indx = 0;
			bool nearby_post = true;

			while (userInput != '0' && indx < posts.Count) 
			{
				if (nearby_post)
				{
					Console.Clear();
					Console.WriteLine(posts[indx]);
					Console.WriteLine("1.Like the post    2. Show omments    3. See new post    0. Go out");
				}
				Console.Write("You select ");

				userInput = Console.ReadLine()[0];
				handleStreamMenuInput(userInput, posts[indx], ref indx, ref nearby_post);

			} 

			if (indx == posts.Count)
			{
				Console.WriteLine("\nIt was the last post in flow\n Select any batton to come back to the main menu.");
				Console.ReadLine();
			}
		}

		private void showFollowFlow()
		{
			string selected;

			Console.Write("Print username ");
			selected = Console.ReadLine();

			if (command.LookIfUsernameIsFollowed(selected))
			{
				var posts = command.GetFlowPosts(selected);

				if (posts.Count == 0)
				{
					Console.WriteLine("This user still didn't write a post.");
					return;
				}

				int indx = 0;
				char userInput = ' ';
				bool nearby_post = true;

				while (userInput != '0' && indx < posts.Count) 
				{
					if (nearby_post)
					{
						Console.Clear();
						Console.WriteLine(posts[indx]);
						Console.WriteLine("1.Like the post    2. Show omments    3. See new post    0. Go out");
					}
					Console.Write("You select  ");

					userInput = Console.ReadLine()[0];
					handleStreamMenuInput(userInput, posts[indx], ref indx, ref nearby_post);

				} 

				if (indx == posts.Count)
				{
					Console.WriteLine("\nIt was the last post in flow\n Select any batton to come back to the main menu.");
					Console.ReadLine();
				}
			}
			else
			{
				Console.WriteLine("You choose wrong username!");
			}
		}
		private void showLookingForMenu()
		{
			char userInput = ' ';
			Console.Clear();

			while (userInput != '0') 
			{
				Console.WriteLine("1. Look for    0. Exit");
				Console.Write("You select  ");

				userInput = Console.ReadLine()[0];
				handleLookingForMenuInput(userInput);

			} 
		}

		private void showUserPostsFlow(User user)
			{
				char userInput = ' ' ;
				Console.Clear();

				var posts = command.GetFlowPosts(user.UserName);

				if (posts.Count == 0)
				{
					Console.WriteLine("This user still didn't write a post.");
					return;
				}

				int index = 0;
				bool next_post = true;

			while (userInput != '0' && index < posts.Count) 
			{
					if (next_post)
					{
						Console.Clear();
						Console.WriteLine(posts[index]);
						Console.WriteLine("1.Like the post    2. Show omments    3. See new post    0. Go out");
					}
					Console.Write("You select  ");

					userInput = Console.ReadLine()[0];
					handleStreamMenuInput(userInput, posts[index], ref index, ref next_post);

				} 

				if (index == posts.Count)
				{
					Console.WriteLine("\nIt was the last post in flow\n Select any batton to come back to the main menu.");
					Console.ReadLine();
				}
			}
		}
	
}
