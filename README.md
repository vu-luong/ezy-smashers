# EzySmashers

An MMO socket game using Ezyfox server and Unity client

![demo2](https://user-images.githubusercontent.com/8142030/138592739-6fa73afd-69a0-492e-82f0-d1917506e1d9.gif)

# How to run?

1. Set up repository:
    - Clone this repo:
      ```
      git clone https://github.com/vu-luong/EzySmashers.git && cd EzySmashers
      ````
    - Clone submodules `ezyfox-server-csharp-client`:
      ```
      git submodule update --init --recursive
      ```
2. Run server:
    - Install mongodb ([Tutorial](https://docs.mongodb.com/manual/administration/install-community/))
    - Create `ezy-smashers` database with `user=root` and `password=123456` using mongo shell:
      ```
        > use ezy-smashers;
        > db.temp.insert({"key": "value"});
        > db.createUser({user: "root", pwd: "123456", roles:[{role: "readWrite", db: "ezy-smashers"}]})
      ```
    - Import ```server``` folder into an IDE (Eclipse, Intellij, Netbean)
    - Run file [ApplicationStartup.java](https://github.com/vu-luong/EzySmashers/blob/master/server/EzySmashers-startup/src/main/java/com/youngmonkeys/ApplicationStartup.java)
3. Run Unity client:
  - Add ```client-unity``` folder to Unity Hub and open it.
  - Open LoginScene: `Assets/1 - Static Assets/1.1 - Scenes/LoginScene.unity`
  - Run LoginScene
  
# Documentation

1. [ezyfox-server](https://youngmonkeys.org/project/ezyfox-sever/)
2. [C# client](https://github.com/youngmonkeys/ezyfox-server-csharp-client)

# Contact us

- Touch us on [Facebook](https://www.facebook.com/youngmonkeys.org)
- Ask us on [stackask.com](https://stackask.com)
- Email to me [Vu Luong](mailto:vubinhcht@gmail.com)

# Help us by donation

Currently, our operating budget is fully from on our own salaries, and all product developments are still based on voluntary contributions from a few members of the organization. Apparently, the low budget would cause many considerable difficulties for us. Therefore, with a clear roadmap and an ambitious goal to provide intellectual products for the community, we really appreciate your support if you can provide a donation to take us further steps. Thanks in advance for your meaningful contributions!

[https://youngmonkeys.org/donate/](https://youngmonkeys.org/donate/)

# License

- Apache License, Version 2.0
