using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChainLogin : MonoBehaviour
{
    public InputField ContractAddressInputField;

    async public void OnLogin()
    {
        int timestamp = (int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
        int expirationTime = timestamp + 60;

        string message = expirationTime.ToString();
        string signature = await Web3Wallet.Sign(message);

        string account = await EVM.Verify(message, signature);
        int now = (int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;

        if (account.Length == 42 && expirationTime >= now) {
            PlayerPrefs.SetString("Account", account);
            PlayerPrefs.SetString("Contract", ContractAddressInputField.text);
            SceneManager.LoadScene("Scenes/Main");
        }
    }
}
