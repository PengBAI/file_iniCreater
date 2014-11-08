using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TP1
	{
	class Program
	{
		static void Main(string[] args)
			{
				Console.Out.WriteLine("Nombre de parametres: " + args.Length);
				
				//Ouverture d'un ficher et lecture d'une valeur
				for(int cpt = 0; cpt < args.Length; cpt++){
					Console.Out.WriteLine(args[cpt]);
				}

				// lecture tous les contenues du fichier
				if(args.Length > 0 && File.Exists(args[0])){
					// lectueur du fichier
					using(StreamReader sr = File.OpenText(args[0])){
						string line;
						while((line = sr.ReadLine()) != null){
							Console.Out.WriteLine(line);
						}
					}
					//sr.Close(); // close fichier
					//sr.Dispose(); // free resources
				}else{
					Console.Out.WriteLine("Fichier non trouvé: " + args[0]);
				}
				IniFile iniFile = new IniFile(args[0]);

				// modifications du fichier
				if(args.Length >= 4){
					try{
						IniFile.Write(args[0], args[1], args[2], args[3]);
					}
					catch(ArgumentException Err)
					{
						Console.Out.WriteLine(Err.Message);
					}
					catch (FileNotFoundException Err)
					{
						Console.Out.WriteLine(Err.Message);
					}
					catch (Exception Err)
					{
						Console.Out.WriteLine(Err.Message);
					}
				}
				Console.In.Read();
		   }
	}
	}
