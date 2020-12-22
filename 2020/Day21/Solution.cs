using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.D21
{
  public record Food(string[] allergens, string[] ingredients);

  public class Solution: ISolution {
  
    public string Name {get;} = "Allergen Assessment";

    public long PartOne(string input) => SolvePartOne(input);
    public long PartTwo(string input) => SolvePartTwo(input);

    private Dictionary<int, Food> FoodList = new Dictionary<int, Food>();
    private Dictionary<string, int> IngredientCount = new Dictionary<string, int>();
    private Dictionary<string, IList<int>> AllergenList = new Dictionary<string, IList<int>>();

    long SolvePartOne(string input) {
      var foods = Parse(input);
      MapFoods(foods);
      var allergens = FindAllergens();
      var allergicIngredients = allergens.SelectMany(v => v.Value).ToHashSet();
      return IngredientCount.Where(e => !allergicIngredients.Contains(e.Key)).Select(e => e.Value).Sum();
    }


    long SolvePartTwo(string input) {
      var allergens = FindAllergens();
      var allergicIngredients = allergens.SelectMany(v => v.Value).ToHashSet();
      while (allergens.Values.Any(i => i.Count > 1)) {
        foreach (KeyValuePair<string, HashSet<string>> entry in allergens.Where(a => a.Value.Count == 1)) {
          foreach (KeyValuePair<string, HashSet<string>> other in allergens.Where(a => a.Value.Count > 1)) {
            if (other.Value.Contains(entry.Value.First())) {
              other.Value.Remove(entry.Value.First());
            }
          }

        }
      }
      Console.WriteLine(String.Join(",", allergens.Keys.OrderBy(s => s).Select(s => allergens[s].First())));
      return 1;
    }

    void MapFoods(IEnumerable<Food> foods) {
      var i = 0;
      foreach(var food in foods) {
        FoodList[i] = food;
        foreach(var allergen in food.allergens) {
          if (!AllergenList.ContainsKey(allergen)) {
            AllergenList[allergen] = new List<int>();
          }
          AllergenList[allergen].Add(i);
        }
        foreach(var ingredient in food.ingredients) {
          if (!IngredientCount.ContainsKey(ingredient)) {
            IngredientCount[ingredient] = 0;
          }
          IngredientCount[ingredient] = IngredientCount[ingredient] + 1;
        }
        i++;
      }
    }

    Dictionary<string, HashSet<string>> FindAllergens() {
      var result = new Dictionary<string, HashSet<string>>();
      foreach(var allergen in AllergenList.Keys) {
        var ingredients = AllergenList[allergen]
          .Select(s => FoodList[s].ingredients);
        var allergicIngredients = ingredients
          .Skip(1)
          .Aggregate(new HashSet<string>(ingredients.First()), (h, i) => {h.IntersectWith(i); return h;}
        );
        result[allergen] = new HashSet<string>(allergicIngredients);
      }
      return result;
    }

    IEnumerable<Food> Parse(string input) {
        return (
          from line in input.Split("\n")
            let parts = line.Split(" (contains ")
            let allergens = parts.Last().Replace(")", "").Split(", ")
            let ingredients = parts.First().Split(" ")
          select new Food(allergens, ingredients)
        );
    }
  }

}