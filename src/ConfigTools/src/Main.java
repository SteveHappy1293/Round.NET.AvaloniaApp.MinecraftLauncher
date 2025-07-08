import SystemHelper.GetConfigPath;
import Windows.MainWindow;

public class Main {
    public static void main(String[] args) {
        String userHome = GetConfigPath.getConfigDir("RoundStudio\\RMCL").toString();
        System.out.println("Get Config Path: " + userHome);

        var window = new MainWindow();
        window.setShow();
    }
}
