# Drinks Info API

Console-based API fetcher to get information about drinks by category. Developed with C#

## Given requirements
* You were hired by restaurant to create a solution for their drinks menu.
* Their drinks menu is provided by an external company. All the data about the drinks is in the companies database, accessible through the CocktailDB API.
* You must create a system that allows the restaurant employee to pull data from any drink in the database.
* User-friendly way to present the data to the employees (the users).
* When the users open the application, they should be presented with the Drinks Category Menu and invited to choose a category. Then they'll have the chance to choose a drink and see information 
 about it.
* There shouldn't be any properties with empty values upon visualization of a given drink.

## Features
* Console based menu where users can choose a drink category.
* Paginated list of drinks in each category for better UX.
* User-friendly means of fetching clearly and elegantly presented data about any drink in the database.

## Challenges
* Implementing extensive mapping of user-input values to make for more lenient matching of drink categories.
* Building the URLs for each API call based on the category was not the most straightforward.
* Avoiding repetition and following DRY.

## Lessons Learned
* Dictionaries are very useful to facilitate mapping.
* Enums are very useful to facilitate user-input handling with minimal future refactoring.
* Caching already fetched data to improve performance is important to UX. 

## Areas To Improve
* Creating a better UX.
* Simplifying code/logic
* DRY

## Resources
* The Cocktail Database