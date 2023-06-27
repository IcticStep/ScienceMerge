# ScienceMerge
## Overview
2d game for android on Unity based on mechanic of merge cards. Development still in progress, most features aren't implemented yet.
Technologies: Extenject(Zenject), UniRx.

Early look of the game:

![early look of the game](https://github.com/IcticStep/ScienceMerge/assets/59373161/d40191cd-a934-40fd-af57-93e60d477de8)

## Setting up
1. Download the project.
2. Open with Unity 2021.3.16f1 or higher.

## Technical details
- The project's architecture is based on dependency inversion through dependency injection, utilizing Extenject (Zenject) for that purpose. Installers are implemented in  [Assets/Scripts/Infrastructure](Assets/Scripts/Infrastructure) and the project context can be found in [Assets/Configuration/Resources](Assets/Configuration/Resources).
- View and model are separated. You may find views in [Assets/Scripts/View](Assets/Scripts/View) and models in [Assets/Scripts/Model](Assets/Scripts/Model).
- Designing pattern "state-machine" is used to make a merge-table in [Assets/Scripts/Model/Merging](Assets/Scripts/Model/Merging).
- UniRx is used to make a timer in [Assets/Scripts/Model/Merging/TablesStates/MergingState.cs](Assets/Scripts/Model/Merging/TablesStates/MergingState.cs).
- In MergeConfigurationTool branch editor window development is currently in progress; in CardsConfigurationTools branch custom inspector is implemented.
