# tower-defense-prototype
A Tower-Defense Prototype developed in 8 hours to show my skills as Unity Developer

## How to Play
1. Click on the icon of the cannon you want to place. (or press 1-2 configurable)
2. Click on the ground to place the cannon.
3. The game ends when either:
  - You defeat all enemies in all waves
  - Or your base runs out of life

## How to Modify the Game

### Level Configuration

In the LevelManager GameObject, you’ll find a component called LevelManager. You can configure the following settings directly in this component: 

* Starting Coins: Set how many coins the player begins with. 
* Reset Cannons on New Wave: Choose whether all cannons should be reset when a new wave starts. 
* Reset Coins on New Wave: Choose whether the player's coins should be reset at the beginning of each wave. 

### Wave Management

In the WaveManager GameObject, there is a WaveManager component. Here you can assign the waves the game will use (currently there are 4 configured). 

### Wave Data

Inside the Data/Waves folder, you'll find all defined waves. You can modify: 

* Which enemies are included
* How many of each enemy type
* The spawn interval between them

To create a new wave: 

1. Right-click in the Project window.
2. Go to Create -> Custom -> Tools -> Wave.
3. Once created, add it to the WaveManager component in the WaveManager GameObject.

### Enemy Configuration

Inside the Data/Enemies folder, you’ll find all available enemy types. You can tweak their attributes directly. 

To create a new enemy: 
1. Right-click in the Project window.
2. Go to Create -> Custom -> Tools -> Enemy -> EnemyData.
3. Once created, add the new enemy asset to the EnemyDatabase located in the Data folder.

### Projectile Configuration

Inside the Data/Projectiles folder, you’ll find all available projectile types. Their attributes can be customized. 
To create a new projectile: 

1. Right-click in the Project window.
2. Go to Create -> Custom -> Tools -> Projectile -> ProjectileData.
3. Once created, add the new projectile asset to the ProjectileDatabase located in the Data folder.

### Cannon Configuration

Inside the Data/Cannons folder, you’ll find all available cannon types. You can edit their properties as needed. 

To create a new cannon: 
1. Right-click in the Project window.
2. Go to Create -> Custom -> Tools -> Cannon -> CannonData.
3. Once created, add the new cannon asset to the CannonDatabase located in the Data folder.
