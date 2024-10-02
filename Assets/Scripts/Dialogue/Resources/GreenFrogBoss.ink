EXTERNAL battleStart()
EXTERNAL npcAction(name)
EXTERNAL questItem(itemName)
EXTERNAL takeQuestItem(itemName)
~ questItem("Snack")
VAR option = 0
VAR questItem = 1
VAR questStatus = ""
-> defeat
=== defeat ===
{questStatus == "complete":
    ~ npcAction("leave")
    ->END
}
{questStatus == "lost":
    ~ option = 3
    Haha
    -> END
}
-> main

=== main ===
You can’t pass unless you’ve got something for me.
I’m feeling a bit... hungry today.
[The giant frog eyes you suspiciously, clearly expecting something in return.]
{questItem == 0: 
    + [Yes, you can have this Snack]
        ~ option = 0
        -> chosen
    + [I have food, but you can't have it!]
        ~ option = 1
        -> chosen
}
{questItem == 1:
    + [No, I don’t have anything for you.]
        ~ option = 1
        -> chosen
}

=== chosen ===
{option == 0:
[The frog's expression softens as he takes the food eagerly.]
Ahh, finally! Much appreciated. You may pass.
[He disappears, allowing you to move forward.]
    ~ takeQuestItem("Snack")
    ~ npcAction("leave")
}
{option == 1:
[The frog narrows his eyes and smirks.]
Then you’ve made your choice. And you...
Have chosen death!
    ~ battleStart()
}
-> END