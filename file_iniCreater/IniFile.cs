using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace TPnet
	{
	class IniFile
		{
		/* stocker les Section et les name=value de fichier .ini dans mémoire */
		private static Hashtable Body;
		/* répertoire de fichier */
		private string Path;
		/* stocker les commentaires par le numéro de la ligne */
		private static Hashtable Commentaires;

		/// <summary>
		/// constructeur par défaut
		/// </summary>
		public IniFile()
			{
			Body = new Hashtable();
			Commentaires = new Hashtable();
			}

		/// <summary>
		/// constructeur pour lire le fhicer .ini et stocker les contenues dans mémoire(Body) 
		/// </summary>
		/// <param name="Path">le répertoire du fichier .ini</param>
		public IniFile(string _Path)
			{
			Path = _Path;
			Body = new Hashtable();
			Commentaires = new Hashtable();
			ReadFile(Path);
			}

		/// <summary>private
		/// lecture du fichier 
		/// </summary>
		/// <param name="Path">le répertoire du fichier</param>
		public void ReadFile(string Path)
			{
			if (File.Exists(Path))
				{
				/* lire le fichier */
				using (StreamReader sr = File.OpenText(Path))
					{
					string Line = "";
					int IndexLine = 0;
					Hashtable NewSection = new Hashtable();
					while ((Line = sr.ReadLine()) != null)
						{
						IndexLine++;
						/* S'il y a commentaire */
						if (Line.Trim().StartsWith(";"))
							{
							Commentaires.Add(IndexLine, Line);
							}
						/* La partie restant */
						if (Line.StartsWith("["))
							{
							/* Section */
							/* stocker le name et le value de même Section dans le même hashtable */
							NewSection = new Hashtable();
							/* add the Section name et tous les contenues de cette Section dans Body hashmap */
							Body.Add(Line.Trim(), NewSection);
							}
						else
							{
							/* Les éléments de la section */
							int Pos = Line.IndexOf("=");
							if (Pos != -1)
								{
								/* récupérer le name */
								string Name = Line.Substring(0, Pos).TrimStart();
								/* récupérer le value */
								string Value = Line.Substring(Pos + 1).TrimStart();
								/* ajouter chaque ligne de la Section à hashtable NewSection */
								NewSection.Add(Name, Value);
								}
							}
						}
					}
				}
			else
				{
				throw new FileNotFoundException(Path + " non trouvé");
				}
			}

		/// <summary>
		/// ecrire le hashtable Body à un fichier
		/// </summary>
		/// <param name="Filename">répertoire un fichier de sortie</param>
		public void WriteToFile(string Filename)
			{
			using (StreamWriter sw = new StreamWriter(Filename, false))
				{
				int IndexCommentaire = 1;
				/* écrire la Section, ex: [Mail] */
				foreach (string Section in Body.Keys)
					{
					/* écrire commentaire */
					while (Commentaires.ContainsKey(IndexCommentaire))
						{
						sw.WriteLine(Commentaires[IndexCommentaire]);
						IndexCommentaire++;
						}
					sw.WriteLine(Section);
					/* écrire commentaire */
					IndexCommentaire++;
					while (Commentaires.ContainsKey(IndexCommentaire))
						{
						sw.WriteLine(Commentaires[IndexCommentaire]);
						IndexCommentaire++;
						}
					/* écrire les éléments de la Section */
					Hashtable sc = (Hashtable)Body[Section];
					if (sc.Count != 0)
						{
						foreach (string Name in sc.Keys)
							{
							/* écrire commentaire */
							while (Commentaires.ContainsKey(IndexCommentaire))
								{
								sw.WriteLine(Commentaires[IndexCommentaire]);
								IndexCommentaire++;
								}
							sw.WriteLine(Name + "=" + sc[Name]);
							/* écrire commentaire */
							IndexCommentaire++;
							while (Commentaires.ContainsKey(IndexCommentaire))
								{
								sw.WriteLine(Commentaires[IndexCommentaire]);
								IndexCommentaire++;
								}
							}
						}
					}
				}
			}

		public string GetString(string Filename, string Section, string Name)
			{
			return GetString(Filename, Section, Name, null);
			}

		public string GetString(string Filename, string Section, string Name, string Default)
			{
			// throw new NotImplementedException();
			if (Body.ContainsKey("[" + Section + "]"))
				{
				Hashtable pSection = (Hashtable)Body["[" + Section + "]"];
				if (pSection.ContainsKey(Name))
					return pSection[Name].ToString();
				else
					{
					//throw new KeyNotFoundException(Name + " non trouvé");
					Console.Out.WriteLine(Name + " non trouvé");
					return "";
					}
				}
			else
				{
				//throw new KeyNotFoundException("Section " + Section + " non trouvé");
				Console.Out.WriteLine("Section " + Section + " non trouvé");
				return "";
				}

			}

		public int GetInteger(string Filename, string Section, string Name)
			{
			return GetInteger(Filename, Section, Name);
			}

		public int GetInteger(string Filename, string Section, string Name, int Default)
			{
			throw new NotImplementedException();
			}

		/// <summary>
		/// Ecrit une valeur dans le ficher ini
		/// </summary>
		/// <param name="Filename">Nom et chemin du ficher ini.</param>
		/// <param name="Section">Nom de la Section.</param>
		/// <param name="Name">Nom de la valeur.</param>
		/// <param name="Value">Contenue de la valeur.</param>
		/// <exception cref="ArgumentException">...</exception>
		/// <exception cref="FileNotFoundException">...</exception>
		public void Write(string Filename, string Section, string Name, object Value)
			{
			if (string.IsNullOrEmpty(Filename))
				{
				throw new ArgumentException("Filename est vide");
				}
			if (string.IsNullOrEmpty(Section))
				{
				throw new ArgumentException("Section est vide");
				}
			if (string.IsNullOrEmpty(Name))
				{
				throw new ArgumentException("Name est vide");
				}
			if (!File.Exists(Filename))
				{
				throw new FileNotFoundException("Fichier non trouvé");
				}
			if (Value is string)
				{
				WriteString(Filename, Section, Name, (string)Value);
				}
			else
				{
				WriteString(Filename, Section, Name, Value.ToString());
				}
			}

		/// <summary>
		/// si Section n'exite pas, alors l'ajouter
		/// sinon la Section exite déja, ajouter le Name=Value
		/// ou modifier la valeur de Name
		/// </summary>
		/// <param name="Filename">repertoire du fichier</param>
		/// <param name="Section">la Section</param>
		/// <param name="Name">le nom</param>
		/// <param name="Value">la valeur</param>
		private void WriteString(string Filename, string Section, string Name, string Value)
			{
			/* si dans le fichier, il exite le Section indiqué */
			if (Body.ContainsKey("[" + Section + "]"))
				{
				/* récuperer le hashtable dans cette Section */
				Hashtable ht = (Hashtable)Body["[" + Section + "]"];
				/* s'il exite le name */
				if (ht.ContainsKey(Name))
					{
					/* modifier la valeur */
					ht[Name] = Value;
					}
				else
					{
					/* ajouter le nouveau élément Name=Value */
					ht.Add(Name, Value);
					}
				}
			else
				{
				/* ajouter nouvelle Section avec le nouveau élément */
				Hashtable ht = new Hashtable();
				ht.Add(Name, Value);
				Body.Add("[" + Section + "]", ht);
				}
			WriteToFile(Filename);
			}

		private void WriteInteger(string Filename, string Section, string Name, int Value)
			{
			if (Body.ContainsKey("[" + Section + "]"))
				{
				Hashtable ht = (Hashtable)Body["[" + Section + "]"];
				if (ht.ContainsKey(Name))
					{
					ht[Name] = Value.ToString();
					}
				else
					{
					ht.Add(Name, Value.ToString());
					}
				}
			else
				{
				Hashtable ht = new Hashtable();
				ht.Add(Name, Value.ToString());
				Body.Add("[" + Section + "]", ht);
				}
			WriteToFile(Filename);
			}

		/// <summary>
		/// supprimer la Section ainsi que tous les éléments
		/// </summary>
		/// <param name="Filename"></param>
		/// <param name="Section">Section's name</param>
		/// <exception cref="KeyNotFoundException">Section non trouvé</exception>
		public void RemoveSection(string Filename, string Section)
			{
			if (Body.ContainsKey("[" + Section.TrimStart() + "]"))
				{
				Body.Remove("[" + Section + "]");
				}
			else
				{
				throw new KeyNotFoundException(Section + " non trouvé!");
				}
			WriteToFile(Filename);
			}

		/// <summary>
		/// supprimer le Name=Value dans une Section
		/// </summary>
		/// <param name="Filename"></param>
		/// <param name="Section"></param>
		/// <param name="name"></param>
		/// <exception cref="KeyNotFoundException">name dans la Section non trouvé</exception>
		/// <exception cref="KeyNotFoundException">Section non trouvé</exception>
		public void RemoveName(string Filename, string Section, string name)
			{
			if (Body.ContainsKey("[" + Section.TrimStart() + "]"))
				{
				Hashtable ht = (Hashtable)Body["[" + Section + "]"];
				if (ht.ContainsKey(name.TrimStart()))
					{
					ht.Remove(name);
					}
				else
					{
					throw new KeyNotFoundException("Dans " + Section + ", " + name + " non trouvé");
					}
				}
			else
				{
				throw new KeyNotFoundException(Section + " non trouvé!");
				}
			WriteToFile(Filename);
			}
		}
	}
