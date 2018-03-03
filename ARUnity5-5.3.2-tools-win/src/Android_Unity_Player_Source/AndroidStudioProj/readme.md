#Android Studio 1.5.+ project To Build UnityARPlayer.jar
###Written By: John Wolf
####Last Updated: 2016-02-02

----

###The "UnityARPlayer.jar" File Build Process

This process has only been tested using Android Studio 1.5.1  installed on Mac OS X.

With a 1.5.1 or greater version of the Android Studio IDE, open the Android Studio project:  

&nbsp;&nbsp;&nbsp;&nbsp;\[ARToolKit for Unity Repo\]/  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;src/Android_Unity_Player_Source/AndroidStudioProj

After the project and the Android Studio IDE is open, check that the SDK and NDK paths are set correctly. (It's a requirement to have the latest Android SDK and NDK downloaded on your devbox.) In the Android Studio IDE, go to "File"/"Project Structure...". In the resulting dialog box, click "SDK Location" in the section along the left margin. In the main window on the right, check the paths on under "Android SDK location:" and "Android NDK location:".

Place any updated dependencies of the following in the following paths:

&nbsp;&nbsp;&nbsp;&nbsp;\[ARToolKit for Unity Repo\]/    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;/src/Android_Unity_Player_Source/AndroidStudioProj/

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;arBaseLib-release/aRBaseLib-release.aar   
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;BT200Ctrl/BT200Ctrl.jar  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;classes/classes.jar

To see the descriptions of these dependencies and where they can be retrieved, see the DAQRI Confluence ARToolKit Space Page for "Creating UnityARPlayer.jar from Source Using Eclipse or Android Studio."  

Build the Java source files to class files by doing: "Build"/"Make Project"

Export the class files as a ".jar" file named "UnityARPlayer.jar": in the right margin of the Android Studio main windows, click and open: "Gradle". This results in a window openning on the right side of the Android Studio IDE labeled "Gradle project". Twirl open "AndroidStudioProj" followed by twirling open another "AndroidStudioProj" icon. Then twirl open "Tasks" followed by "other." Scroll to "jarRelease." Double click "jarRelease" from the "Gradle projects" windows.  This collects the built class files and packages them into:

&nbsp;&nbsp;&nbsp;&nbsp;\[ARToolKit for Unity Repo\]/src/Android_Unity_Player_Source/   
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;AndroidStudioProj/UnityARPlayer/build/libs/UnityARPlayer.jar

###Placing the "UnityARPlayer.jar" File

Copy the updated "UnityARPlayer.jar" file:

from

&nbsp;&nbsp;&nbsp;&nbsp;\[ARToolKit for Unity Repo\]/src/Android_Unity_Player_Source/   
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;AndroidStudioProj/UnityARPlayer/build/libs/UnityARPlayer.jar

to

&nbsp;&nbsp;&nbsp;&nbsp;\[ARToolKit for Unity Repo\]/src/Unity/Assets/Plugins/Android/

Task Complete.