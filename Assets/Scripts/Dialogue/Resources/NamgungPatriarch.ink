EXTERNAL npcAction(name)
EXTERNAL battleStart()
EXTERNAL getQuestStatus(questName)
EXTERNAL setQuestStatus(questName, questStatus)
EXTERNAL getMainQuestStatus()
VAR option = 0
VAR questStatus = ""
VAR mainQuestAmount = 0

-> fightover

=== fightover ===
{questStatus == "complete":
So, you’ve learned something after all... Impressive.
As promised, I will make my way to the Alliance. A Namgung Patriarch does not break his word.
[He turns toward your brother, Jiho.]
And Jiho, prepare yourself. You will train relentlessly for a whole week when I return.
    ~ setQuestStatus("questNamgung", "success")
    * [I shall see you at the headquarters]
    ~ getMainQuestStatus()
    Indeed, you shall. Until then.
    -> END
}
{questStatus == "lost":
Pathetic! The Alliance has made you weaker, not stronger.
As we agreed, I will stay with the clan. The Alliance will have to face this threat without the Namgung Clan’s strength.
[He sneers and gives a dismissive wave.]
Go back to your so-called 'Heavenly Sword' and tell him you failed. 
And when they cast you out of the Alliance... Remember, this will always be your home, even in disgrace.
    *[Goodbye father]
    Goodbye son
    ~ setQuestStatus("questNamgung", "fail")
    -> END
}
~ getQuestStatus("questNamgung")
-> main

=== main ===
~ getQuestStatus("questNamgung")
{questStatus == "unobtained":
Ah, Jin, my son.
Have you finally come to your senses and decided to return to your family where you belong?
    *[I'm here to deliver a message from the Alliance]
        Still acting as the dog of the Alliance I see...
        Go on then, speak.
        **[The Demonic sect is making a return]
        ***[We need your assistance father]
        [The patriarch’s eyes narrow as he considers your words]
        The Demonic Sect, you say? This is indeed a grave matter...
        [He pauses, then leans forward, his gaze stern.]
        But tell me, Jin, why should I abandon my clan to assist the Alliance? My first duty is to the Namgung family!
        You need to grow stronger, like your elder brothers. Abandon the Alliance and return to your rightful place.
        ****[I will not]
        [He sighs, shaking his head.]
        Stubborn as ever... Very well, I shall make you an offer, then...
        Duel with your brother Jiho. Prove to me that the Alliance has not made you soft and weak.
        If you can defeat him, I will leave for the Alliance immediately.
        But should you lose, I will refuse their call, and the blame for our absence will rest squarely on your shoulders.
        *****[I accept your proposition]
        [The patriarch turns to Jiho.]
        Jiho, teach your younger brother the lesson he so desperately needs.
        ~ battleStart()
        -> END
}
{questStatus == "success":
I will depart for the Alliance soon. Leave me now, Jin
-> END
}
{questStatus == "fail":
How much longer do you think you’ll survive in this world, Jin? Time will tell.
-> END
}

