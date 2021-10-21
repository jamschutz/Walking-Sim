# Walking-Sim

# Git Worfkflow

### You made changes, and want to push them up

```bash
# this will “stage” all of your files -- essentially, it tells git that you created some brand new files that it might not know about, so you’re asking git to look for any new files you might have made
git add .

# this will “commit” your files -- basically, this tracks all the changes you’ve made in any files, and git attaches a little message to say what you’ve been up to
git commit -am "YOUR COMMIT MESSAGE"

# this will “push” your files up to GitHub -- now everyone can access your latest changes!
git push

# NOTE: the first time you do this, you have to tell git what to call your branch on GitHub (you should call this the same thing you called it on your computer!). to do this, run this command:
git push -u origin NAME_OF_YOUR_BRANCH
```


### You want to get the latest updates to dev into your branch

```bash
#this will get the latest updates to the dev branch on GitHub (anything someone else pushed up there since you last looked), package those changes and bring them into your branch
git pull origin dev
```


### You want to look at someone else’s branch

```bash
# this will change the project to what the other person is working on.
git checkout NAME_OF_BRANCH

# git may complain and say you have uncommitted changes. in that case, just commit your current changes before switching branchehs by doing this:
git add .
git commit -am "YOUR COMMIT MESSAGE"
git checkout NAME_OF_BRANCH
```