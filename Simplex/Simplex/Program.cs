Console.WriteLine("Welcome to simplex method console!\n");

string[] textLines = File.ReadAllLines("Examples/2x2.csv");

Console.WriteLine("Function to optimize:");
Console.WriteLine(textLines[0]);

Console.WriteLine("Conditions:");

for (int i = 1; i < textLines.Length; i++)
{
	Console.WriteLine(textLines[i]);
}

Console.ReadLine();
