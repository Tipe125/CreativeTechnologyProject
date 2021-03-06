Behavioural Analysis Tools
Creative Technology Project - Tim Penfold (15010658)


Installation Instructions:

1) Once the project has been downloaded, put each folder excluding the scenes folder
(Prefabs, Screenshots, Scripts, SQLite Files and Tracking) into the desired Unity 
project's Assets folder. The scenes folder is not needed as it only contains an 
example scene to demonstrate the tools. The example scene can also be used for 
reference when setting up the tools.

The Tools will now be ready for use within your Unity Project.

How to use the Tools:

Object Tracker - The Object Tracker serves the purpose of populating the Database with
information on playtest runs to create heatmaps and files containing Data 
from a given run, including the tracked objects transforms and rotation. This is done 
every time a set number of frames pass, which can be chosen by the user by adjusting the
interval rate. To use the object tracker, place it in the scene and attach the object you 
wish to track in the 'Tracked Object' variable within the inspector. Then choose how often 
you want data to be tracked by entering the interval rate. Lower Intervals will acquire more 
data and show smaller changes but may impact performance. The default is 60, which should run 
efficiently on most machines.

Files are created by the object tracker when a session is started (when the user enters 
play mode in Unity) and are saved to the 'Tracking' folder. 

Snapshot Camera - The Snapshot Camera is used by the Database Interface object to 
create Heatmaps by saving it's current field of view to a PNG File. This should be 
placed into the scene above the map or in such a way that its field of view contains
the entire map (or the portion of the map you wish to create a heatmap for). There is
also an array within the object called 'Invisible Objects'. This is used to disable any
objects that may obstruct the view of the snapshot camera. To use this, change the number
of elements in the array within the inspector to the desired number and then place each 
object that you wish to be invisible into the elements. 

Images created by the Snapshot Camera are saved to the 'Screenshots' folder.

Custom Events - The Custom Event script can be attached to any object and allows you
to designate a term to be entered into the database when a collision event occurs or
an object is destroyed. To use custom events, attach the Custom Event script to the 
object you want to have custom events entered for as well as the object tracker in the
scene. After that, tick the checkboxes for any events you'd like to utilise and enter 
the word or phrase you'd like to see in the entry in the textbox below the respective
checkbox. For example, if you tick the 'collision entered' checkbox and enter 'Crashed'
into the textbox below it, whenever the object collides with another object an entry will
be created with the word 'Crashed' in the Custom Event Column. 

In any heatmaps created, entries involving a custom event will be shown using a green point 
rather than the usual red shades. 

Database Interface - The Database Interface utilises the Databases populated by the 
Object Tracker and the Snapshot Camera to create heatmaps visualising the transforms
of the tracked object in one or several runs. To set up the Database Interface, place
it and the Snapshot Camera into the scene. Then place the Snapshot Camera into the 'Cam'
variable in the Database Interface object within the inspector. The Database Interface 
will also need data to create heatmaps from, so if the Object Tracker hasn't already been 
placed put it into the scene and attach the object that you wish to track. 

Once you have started a run, the database interface will now appear onscreen. You will have
the option to choose how many runs to create a heatmap from based on the number of runs
within the database. Based on the number chosen, a number of dropdowns will appear onscreen.
These dropdowns are used to choose which runs you wish to use data from when creating the 
heatmap. Once you have chosen which runs to use data from, click the 'Generate Heatmap' button.
This will create a heatmap which will be stored within the 'Screenshots' folder. 

You can also create a heatmap using one of the two set queries - 'All Runs from Date' and 'All
Runs with Event'. The former lets you choose from the dates of every run in the database and 
generate a heatmap from all runs performed on that date. The latter allows you to find all runs
that have an entry with the chosen custom event and generate a heatmap using those runs. 

Playback Bot - The Playback Bot uses the RunData files created by the Object Tracker as reference
to simulate the movements of the tracked object in a given run. To use the Playback Bot you will 
require atleast one RunData file, so place the Object Tracker in your scene and attach the object
you wish to track in the inspector to begin creating RunData files. To use the Playback Bot, place
it in the scene at the starting location of the object that was tracked and place the desired RunData
file (found in the 'Tracking' folder') in the 'Run Data File' variable within the inspector. Once that 
is done, if you enter play mode and start a run, the Playback Bot will move based on the the data
contained in the RunData file chosen. 


Potential Issues:

- You'll need to click off of Unity and then back on to update the folders within the asset directory. This
is relevant when new screenshots or Rundata files are added to the directory.

- On some machines, the screenshots taken by the Snapshot Camera will not open in the Photos application.
They are still viewable within Unity and in other programs such as Paint. If this problem occurs, use the
aforementioned programs to view the images instead. 

- In some projects, the first run will not allow for use of the database functionality. That run is still being recorded
and the next time you enter play mode the run will be there for you to use as desired.



Other Information:

- To reset the Database, delete or move the files 'RunData.sqlite' and 'RunID.sqlite' from the project directory (in 
the same folder the Assets folder is contained within). When a run of a scene containing the Object Tracker is played,
these files will be created again as empty databases, which will then be populated as runs accumulate. 








