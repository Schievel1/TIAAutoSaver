Licenced under GNU GPL V3 https://www.gnu.org/licenses/gpl-3.0.en.html

THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

# TIAAutoSaver
TIAAutoSaver is a simple project that automatically saves instances of Siemens' PLC IDE "TIA Portal" in a costum interval. 

Run TIAAutoSaver.exe alongside TIA-Portal. TIA Portal instances will automatically appear in the list on the left side, as soon as a project is loaded in TIA Portal.

- To activate autosave for a single instance, mark the instance by clicking on it an click the ">" button. This will move the instance to the right list. Every instance on the right list is automatically saved.
- To add all instances to the right list, press the ">>" button. 
- To remove instances from the right list, mark the instance and press the "<" button. 
- Similarily to remove all instances on the right list, press the "<<" button. 
- Instances are automatically updated every 5 seconds. If a TIA Portal instance is closed or a project is unloaded, the instance is removed from either of the list. 
- To update the instaces manually, press the "Re" button
- You can change the interval in which the instaces are saved with the UpDown-Buttons. However, the last interval needs to finish before the TIA instances are saved an     the next (changed) interval starts. 
- The right list shows the last time a TIA Portal Project was saved. In TIA Portal a project can only be saved when something was changed since the last time the project was saved. Therefore the "Last saved" tag is not updated, if nothing was changed. 

Please keep in mind, that saving removes the ability to undo things in TIA Portal. 
Also saving takes a while on older versions on TIA Portal, so you should not add too many instances to the autosave list. 

# TIAAutoSaveStarter
TIAAutoSaveStarter is an additional small program that lets you create a shortcut to start a new TIA Portal instance and automatically add it to the list of autosave instances. 

- TIAAutoSaveStarter.exe needs to remain in the same folder like TIAAutoSave.exe.
- Create a shortcut to TIAAutoSaveStarter.exe and pass the path to the TIA Portal .exe as argument. Note that a path with white spaces in it needs to be passed in double quotes. The target auf the shortcut needs to be something like this:
"C:\your\path\to\tiaautasavestarter\TIAAutoSaveStarter.exe" "C:\your\path\to\tiaportal\Siemens\Automation\Portal V16\Bin\Siemens.Automation.Portal.exe"
