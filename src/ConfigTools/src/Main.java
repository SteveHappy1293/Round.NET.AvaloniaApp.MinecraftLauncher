import SystemHelper.GetConfigPath;

public class Main {
    public static void main(String[] args){
        String userHome = GetConfigPath.getConfigDir("RoundStudio\\RMCL").toString();
        System.out.println("Get Config Path: "+userHome);
    }
}
