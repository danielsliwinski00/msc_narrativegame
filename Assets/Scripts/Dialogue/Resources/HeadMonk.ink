EXTERNAL npcAction(name)
EXTERNAL getQuestStatus(questName)
EXTERNAL setQuestStatus(questName, questStatus)
~ getQuestStatus("questShaolin")
VAR option = 0
VAR questStatus = "unobtained"
~ getQuestStatus("questShaolin")
-> main

=== main ===
~ getQuestStatus("questShaolin")
{questStatus == "unobtained":
Greetings, traveler.
*[I greet the Head Monk of the Shaolin]
Tell me, what brings you to the sacred grounds of Shaolin?
    **[I'm here to deliver a message]
        A message? And from whom, might I ask? Who are you, traveler?
            ** *[It's from the Alliance leader]
                The Martial Alliance... I see. Please, share the message with me.
                    ****[The Demonic sect is back]
                    *****[We need your presence at the Alliance headquarters.]
                    -> dialogue()
            *** [I'm Namgung Jin from the Martial Alliance]
                Ah, a son of the Namgung Clan. Your name carries great weight. What message do you bring from the Alliance?
                    ****[The Demonic sect is back]
                    *****[And we need you to come to the headquarters]
                    -> dialogue()
}
{questStatus == "incomplete":
Have you killed the bamboo monster?
    + [I'm working on it]
        ~ option = 0
        -> chosen
    + [No, I give up]
        ~ option = 1
        -> chosen
}
{questStatus == "completeW":
~ setQuestStatus("questShaolin", "success")
Have you killed the bamboo monster?
    * [The monster is alive no more!]
    [The monk bows deeply in gratitude.]
    You have done a great service, not only to the villagers but to all of Shaolin. As promised, I will join the Alliance. Amitabha.
    -> END
}
{questStatus == "completeF":
~ setQuestStatus("questShaolin", "fail")
Have you killed the monster?
    * [I have been bested, but he ran away]
    But for how long will he be gone?
    ** [I'm lucky to get away with my life!]
        ~ option = 1
        -> chosen
}
{questStatus == "success":
I will be on my way as soon as possible. Amitabha.
-> END
}
{questStatus == "fail":
I do not wish to speak to you anymore
Please leave the temple
-> END
}


=== dialogue ===
~ setQuestStatus("questShaolin", "incomplete")
[The Head Monk’s expression becomes grave, his voice solemn.]
    The Demonic Sect... troubling news indeed.
    In fact, we have recently received word from the villagers at the base of the temple.
    A giant bamboo monster has appeared in the bamboo forest to the east, wreaking havoc!
    [The Head monk pauses, his expression darkening.]
    After hearing about the demonic sect making a return...I fear this creature may be connected to their dark magic.
    If you can defeat this monster and bring peace to the villagers, I will be free of worry and join the Alliance without hesitation.
    * [I shall do my best]
    [The monk bows slightly, his eyes filled with resolve.]
    Amitabha. May your path be clear, and may you succeed.
    I will inform the guards to allow you passage. May the Buddha guide you on your journey
    -> END
    
=== chosen ===
{option == 0:
    [The head monk nods but seems slightly impatient.]
    Don't take too long now. The villagers are suffering.
    -> END
}
{option == 1:
[The Head Monk frowns, his disappointment evident.]
    I'm disappointed
    I didn't know the Alliance was this weak.
    Looks like I'll have to stay here and protect the Shaolin instead.
    ~ setQuestStatus("questShaolin", "fail")
    -> END
}

