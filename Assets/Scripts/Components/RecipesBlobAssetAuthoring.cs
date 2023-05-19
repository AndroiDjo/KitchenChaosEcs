using Unity.Collections;
using UnityEngine;
using Unity.Entities;

public class RecipesBlobAssetAuthoring : MonoBehaviour {
    public RecipeListSO recipeListSo;

    class Baker : Baker<RecipesBlobAssetAuthoring> {
        public override void Bake(RecipesBlobAssetAuthoring authoring) {
            var builder = new BlobBuilder(Allocator.Temp);
            ref RecipeList recipeList = ref builder.ConstructRoot<RecipeList>();
            BlobBuilderArray<Recipe> recipeBuilder = builder.Allocate(ref recipeList.Recipes, authoring.recipeListSo.recipeSOList.Count);
            for (int i = 0; i < authoring.recipeListSo.recipeSOList.Count; i++) {
                RecipeSO recipeSo = authoring.recipeListSo.recipeSOList[i];
                recipeSo.ingredients.Sort();
                
                BlobBuilderArray<IngredientType> ingredientsBuilder =
                    builder.Allocate(ref recipeBuilder[i].Ingredients, recipeSo.ingredients.Count);
                for (int j = 0; j < recipeSo.ingredients.Count; j++) {
                    ingredientsBuilder[j] = recipeSo.ingredients[j].ingredientType;
                }
                builder.AllocateString(ref recipeBuilder[i].RecipeName, recipeSo.recipeName);
            }

            var blobReference = builder.CreateBlobAssetReference<RecipeList>(Allocator.Persistent);
            builder.Dispose();
            
            AddBlobAsset<RecipeList>(ref blobReference, out var hash);
            
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new RecipesListComponent{RecipesReference = blobReference});
        }
    }
}

public struct Recipe {
    // Sorted list of ingredients.
    public BlobArray<IngredientType> Ingredients;
    public BlobString RecipeName;
}

public struct RecipeList {
    public BlobArray<Recipe> Recipes;
}

public struct RecipesListComponent : IComponentData {
    public BlobAssetReference<RecipeList> RecipesReference;
}
