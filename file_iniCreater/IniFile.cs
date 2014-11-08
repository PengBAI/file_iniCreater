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
			private Hashtable body;

			/* constructeur pour lire le fhicer .ini et stocker les contenues dans mémoire(body) */
			public IniFile(string path){
				if(File.Exists(path)){
					body = new Hashtable();

					using (StreamReader sr = File.OpenText(path)){
						string line = "";
						string sectionName = "";
						bool firstTime = true;
						/* stocker le name et le value de même section dans le même hashtable */
						Hashtable newSection = new Hashtable(); 
						while ((line = sr.ReadLine()) != null){
							if (line.StartsWith("["))
							{
								if(firstTime){
									sectionName = line;
									firstTime = false;
								}else{
									/* add the section name et tous les contenues de cette section dans body hashmap */
									body.Add(sectionName,newSection);
									/* mise à jour le section name */
									sectionName = line;
									/* alouer nouvelle mémoire pour nouveaux contenues de la section */
									newSection = new Hashtable();
								}
								continue;
							}else{
								int pos = line.IndexOf("=");
								if(pos != -1){
									/* récupérer le name */
									string name = line.Substring(0,pos).Trim();
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
				}else{
					throw new FileNotFoundException(path + " non trouvé");
				}
			
			}
		
			public static string GetString(string Filename, string Section, string Name)
			{
				return IniFile.GetString( Filename, Section, Name, null);
			}

			public static string GetString(	string Filename, string Section, string Name, string Default)
			{
				throw new NotImplementedException();
            }
			public static int GetInteger(string Filename, string Section, string Name)
			{
				return IniFile.GetInteger( Filename, Section, Name);
			}

			public static int GetInteger(string Filename, string Section, string Name, int Default)
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
			public static void Write(string Filename, string Section, string Name, object Value)
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

			public static void WriteString(string Filename, string Section, string Name, string Value)
			{
			}

			public static void WriteInteger(string Filename, string Section, string Name, int Value)
			{
			}
		}
	}
