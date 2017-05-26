# playing-nice-together
Playing nice together: how to use F# in a brownfield project

Like many companies, our main code base is a C# monolith. Although there is a lot of domain knowledge captured in it, using C# wasn't always the best choice to solve our domain problems. When we discovered F#, we realised that it was a better fit for some of the features we were currently implementing in C#. However, rewriting everything at once in F# would be ineffective. The manual says C# and F# play nice together. So we tried that out, pushing it as far as we could.

In this talk I will show you how we used F# in our existing C# monolith. I will talk about the positive and negative effects of our decisions, what I would do differently in the future and whether or not C# and F# do indeed play nice together.
