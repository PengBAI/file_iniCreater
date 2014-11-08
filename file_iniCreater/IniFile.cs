using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace TP1
	{
	class IniFile
		{
			/* stocker les section et les name=value de fichier .ini dans mémoire */
			private static Hashtable body;
			private string path;
			/// <summary>
			/// constructeur pour lire le fhicer .ini et stocker les contenues dans mémoire(body) 
			/// </summary>
			/// <param name="path">le répertoire du fichier .ini</param>
			public IniFile(string _path){
				path = _path;
				readFile(path);
			}

			/// <summary>
			/// lecture du fichier 
			/// </summary>
			/// <param name="path">le répertoire du fichier</param>
			private void readFile(string path){
				if (File.Exists(path))
					{
					body = new Hashtable();

					using (StreamReader sr = File.OpenText(path))
						{
						string line = "";
						string sectionName = "";
						bool firstTime = true;
						/* stocker le name et le value de même section dans le même hashtable */
						Hashtable newSection = new Hashtable();
						while ((line = sr.ReadLine()) != null)
							{
							if (line.StartsWith("["))
								{
								if (firstTime)
									{
									sectionName = line;
									firstTime = false;
									}
								else
									{
									/* add the section name et tous les contenues de cette section dans body hashmap */
									body.Add(sectionName, newSection);
									/* mise à jour le section name */
									sectionName = line;
									/* alouer nouvelle mémoire pour nouveaux contenues de la section */
									newSection = new Hashtable();
									}
								continue;
								}
							else
								{
								int pos = line.IndexOf("=");
								if (pos != -1)
									{
									/* récupérer le name */
									string name = line.Substring(0, pos).Trim();
									/* récupérer le value */
									string value = line.Substring(pos + 1).Trim();
									/* ajouter chaque ligne de la section à hashtable newSection */
									newSection.Add(name, value);
									}
								}
							}
						/* ajouter le dernière section à body */
						body.Add(sectionName, newSection);
						}
					}
				else
					{
					throw new FileNotFoundException(path + " non trouvé");
					}
			}
		
			public string GetString(string Filename, string Section, string Name)
			{
				return GetString( Filename, Section, Name, null); 
			}

			public string GetString(string Filename, string Section, string Name, string Default)
			{
				// throw new NotImplementedException();
				if(body.ContainsKey("["+Section+"]")){
					Hashtable section = (Hashtable)body["["+Section+"]"];
					if(section.ContainsKey(Name))
					    return section[Name].ToString();
					else{
						//throw new KeyNotFoundException(Name + " non trouvé");
						Console.Out.WriteLine(Name + " non trouvé");
						return "";
						}
				}else{
					//throw new KeyNotFoundException("section " + Section + " non trouvé");
					Console.Out.WriteLine("section " + Section + " non trouvé");
					return "";
				}
				
            }
			public int GetInteger(string Filename, string Section, string Name)
			{
				return GetInteger( Filename, Section, Name);
			}

			public int GetInteger(string Filename, string Section, string Name, int Default)
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// Ecrit une valeur dans le ficher ini
			/// </summary>
			/// <param name="Filename">Nom et chemin du ficher ini.</param>
			/// <param name="Section">Nom de la section.</param>
			/// <param name="Name">Nom de la valeur.</param>
			/// <param name="Value">Contenue de la valeur.</param>
			/// <exception cref="ArgumentException">...</exception>
			/// <exception cref="FileNotFoundException">...</exception>
			public void Write(string Filename, string Section, string Name, object Value)
			{
				if( string.IsNullOrEmpty(Filename)){
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
				if(!File.Exists(Filename))
				{
					throw new FileNotFoundException("Fichier non trouvé");
				}
				if(Value is string){
					WriteString( Filename, Section, Name, (string)Value);
				}else{
					WriteString(Filename, Section, Name, Value.ToString());
				}

			}
			/// <summary>
			/// si section n'exite pas, alors l'ajouter
			/// sinon la section exite déja, ajouter le Name=Value
			/// ou modifier la valeur de Name
			/// </summary>
			/// <param name="Filename">repertoire du fichier</param>
			/// <param name="Section">la section</param>
			/// <param name="Name">le nom</param>
			/// <param name="Value">la valeur</param>
			private void WriteString(string Filename, string Section, string Name, string Value)
			{
				/* si dans le fichier, il exite le section indiqué */
				if(body.ContainsKey("["+Section+"]")){
					/* récuperer le hashtable dans cette section */
					Hashtable ht = (Hashtable)body["["+Section+"]"];
					/* s'il exite le name */
					if(ht.ContainsKey(Name)){
						/* modifier la valeur */
						ht[Name] = Value;
					}else{
					    /* ajouter le nouveau élément Name=Value */
						ht.Add(Name, Value);
					}
				}else{
					/* ajouter nouvelle section avec le nouveau élément */
					Hashtable ht = new Hashtable();
					ht.Add(Name, Value);
					body.Add("["+Section+"]", ht);
				}
				writeToFile(Filename);
			}

			private void WriteInteger(string Filename, string Section, string Name, int Value)
				{
				if (body.ContainsKey("[" + Section + "]"))
					{
					Hashtable ht = (Hashtable)body["[" + Section + "]"];
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
					body.Add("["+Section+"]", ht);
					}
				writeToFile(Filename);
				
			}

			/// <summary>
			/// ecrire le hashtable body à un fichier
			/// </summary>
			/// <param name="Filename">répertoire un fichier de sortie</param>
			private void writeToFile(string Filename){
			using (StreamWriter sw = new StreamWriter(Filename, false))
				{
					/* écrire la section, ex: [Mail] */
					foreach (string section in body.Keys)
					{
						sw.WriteLine(section);
						/* écrire les éléments de la section */
						Hashtable sc = (Hashtable)body[section];
						if(sc.Count != 0){
							foreach(string name in sc.Keys){
								sw.WriteLine(name+"="+sc[name]);
							}
						}
					}
				}
			}
		}
	}
