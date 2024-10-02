-> main
VAR choice = 0
VAR playerOrigin = 1

=== main ===
Hello sir
    Did you come here for the shaolin temple?
    + [Yes where is it?]
    ~ choice = 1
    -> chosen
    + [*ignore*]
    ~ choice = 2
    -> chosen

=== chosen ===
{choice == 1:
    It's just straight up ahead!
    Excuse me sir...
    You look strong...
    Would you please defeat the bamboo monster...
    Dad says we can't eat much because he has no bamboo to sell...
    Or if you can't defeat it, at least ask the shaolin to kill it! Please!
}
{choice == 2:
    Oh...
    I'm hungry...
}
-> END