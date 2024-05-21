// Programmer's Name: Han Phan
// Project Name: Phan_3
// Due Date: 04/05/2024
// Project Description: Assignment 3
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Phan_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Dclare constant
        private const decimal TAX_RATE = 0.07m;
        private const decimal EXTRA_PRICE = 9.95m;
        private const decimal HOME_DELIVERY_PRICE = 7.50m;
        private const decimal SINGLE_PRICE = 9.95m;
        private const decimal HALFDOZEN_PRICE = 35.95m;
        private const decimal DOZEN_PRICE = 65.95m;
        private const decimal PERSONALIZED_MESSAGE_PRICE = 2.50m;

        // Decalre variables
        private decimal orderTotal;
        private decimal subTotal;
        private decimal tax;
        private string deliveryType;
        private string size;
        private string message = "No Message";
        private string selectedItemList = " ";
        private int itemCount;


        // Method to populate box data
        private void PopulateBoxes(string fileListBox, string fileComboBox)
        {
            try
            {
                string[] listBoxLines = File.ReadAllLines(fileListBox);
                string[] comboBoxLines = File.ReadAllLines(fileComboBox);
                foreach (string line in listBoxLines)
                {
                    extraListBox.Items.Add(line);
                }
                foreach (string line in comboBoxLines)
                {
                    occassionsComboBox.Items.Add(line);
                }
                extraListBox.Sorted = true;
                occassionsComboBox.Sorted = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Reset Form Method
        private void ResetForm()
        {
            tittleComboBox.SelectedIndex = -1;
            firstNameTextBox.Text = string.Empty;
            lastNameTextBox.Text = string.Empty;
            streetTextBox.Text = string.Empty;
            stateMaskedTextBox.Text = string.Empty;
            zipMaskedTextBox.Text = string.Empty;
            phoneMaskedTextBox.Text = string.Empty;
            storePickupRadioButton.Checked = true;
            singleRadioButton.Checked = true;
            occassionsComboBox.SelectedIndex = 1;
            extraListBox.SelectedIndex = -1;
            personalizedMessCheckBox.Checked = false;
            personalizedMessTextBox.Text = string.Empty;
            personalizedMessTextBox.Enabled = false;
            tittleComboBox.Focus();
            updateTotal();
        }

        // Method to update total
        private void updateTotal()
        {
            subTotalLabel.Text = subTotal.ToString("c");
            tax = subTotal * TAX_RATE;
            taxLabel.Text = tax.ToString("c");
            orderTotal = tax + subTotal;
            oederTotalLabel.Text = orderTotal.ToString("c");
        }
           
        // Update Delivery
        private void storePickupRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            updateTotal();
            deliveryType = "Store Pickup";
        }

        private void homeDeliveryRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (homeDeliveryRadioButton.Checked)
            {
                subTotal += 7.50m;
                deliveryType = "Home Delivery";
            }
            else
            {
                subTotal -= 7.50m;
            }
            updateTotal();
        }

        // Update Size Bundle
        private void singleRadioButton_CheckedChanged(object sender, EventArgs e)
        {   
            if (singleRadioButton.Checked)
            {
                subTotal += SINGLE_PRICE;
                size = "Single";
            }
            else
            {
                subTotal -= SINGLE_PRICE;
            }
            updateTotal();
        }
        private void halfdozenRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (halfdozenRadioButton.Checked)
            {
                subTotal += HALFDOZEN_PRICE;
                size = "Half Dozen";
            }
            else
            {
                subTotal -= HALFDOZEN_PRICE;
            }
            updateTotal();
        }
        private void dozenRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (dozenRadioButton.Checked)
            {
                subTotal += DOZEN_PRICE;
                size = "Dozen";
            }
            else
            {
                subTotal -= DOZEN_PRICE;
            }
            updateTotal();
        }

        // Extra List Box Update
        private void extraListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedItemList = string.Empty;
            subTotal -= (EXTRA_PRICE * itemCount);
            itemCount = 0;

            for (int i = 0; i < extraListBox.Items.Count; i++)
            {
                if (extraListBox.GetSelected(i))
                {
                    selectedItemList += "\n" + " - " + extraListBox.Items[i];
                    itemCount++;
                    subTotal += EXTRA_PRICE;
                }
            }

            updateTotal();
        }

        // Update Personalized Mess
        private void personalizedMessCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (personalizedMessCheckBox.Checked)
            {
                personalizedMessTextBox.Enabled = true;
                subTotal += 2.50m;
                limitCharacterLabel.Enabled = true;
            }
            else
            {
                personalizedMessTextBox.Enabled = false;
                subTotal -= 2.50m;
                limitCharacterLabel.Enabled = false;
            }
            updateTotal();
        }
        private void personalizedMessTextBox_TextChanged(object sender, EventArgs e)
        {
            message = personalizedMessTextBox.Text;
        }

        // Display Summary Button
        private void displaySummaryButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(firstNameTextBox.Text) || string.IsNullOrEmpty(lastNameTextBox.Text) || !phoneMaskedTextBox.MaskCompleted)
            {
                MessageBox.Show("Enter First Name, Last Name and appropriate phone number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!string.IsNullOrEmpty(firstNameTextBox.Text) && !string.IsNullOrEmpty(lastNameTextBox.Text) && phoneMaskedTextBox.MaskCompleted)
            {
                MessageBox.Show("Customer Name: " + firstNameTextBox.Text + " " +  lastNameTextBox.Text
                + "\n" + "Customer Adress: " + streetTextBox.Text + " " + stateMaskedTextBox.Text + " " + zipMaskedTextBox.Text
                + "\n" + "Customer Phone Number: " + phoneMaskedTextBox.Text.ToString()
                + "\n" + "Delivery Date: " + desiredDeliveryMaskedTextBox.Text.ToString()
                + "\n" + "Delivery Type: " + deliveryType
                + "\n" + "Size Bundle: " + size
                + "\n" + "Occassion: " + occassionsComboBox.SelectedItem.ToString()
                + "\n" + "Extras: " + selectedItemList
                + "\n" + "Message: " + message.ToString()
                + "\n"
                + "\n" + "Order Subtotal: " + subTotal.ToString("c")
                + "\n" + "Sale Tax Amount: " + tax.ToString("c")
                + "\n" + "Order Total: " + orderTotal.ToString("c"),
                "Bonnie’s Balloons Order Summary");
                ResetForm();
            }
                    
        }

        // Setting up Clear Button
        private void clearFormButton_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        // Setting up Exit Button
        private void exitButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit the program?", "Exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                this.Close();
            }          
        }

        // Setting up Form Load 
        private void Form1_Load(object sender, EventArgs e)
        {
            storePickupRadioButton.Checked = true;
            singleRadioButton.Checked = true;
            PopulateBoxes("Extras.txt", "Occassions.txt");
            desiredDeliveryMaskedTextBox.Text = DateTime.Now.ToString("MM/dd/yyyy");
            occassionsComboBox.SelectedIndex = 1;
            limitCharacterLabel.Enabled = false;
            homeDeliveryPriceLabel.Text = ("( " + HOME_DELIVERY_PRICE.ToString("c") + " fee)");
            singlePriceLabel.Text = ("- " + SINGLE_PRICE.ToString("c"));
            halfdozenPriceLabel.Text = ("- " + HALFDOZEN_PRICE.ToString("c"));
            dozenPriceLabel.Text = ("- " + DOZEN_PRICE.ToString("c"));
            personalizeMessagePriceLabel.Text = ("- " + PERSONALIZED_MESSAGE_PRICE.ToString("c"));
            extraPriceLabel.Text = ("- " + EXTRA_PRICE.ToString("c") + " for each");
            taxRateLabel.Text = ("(" + TAX_RATE.ToString("p") + ")");

            // Setting Tool Tip
            displaySummaryToolTip.SetToolTip(this.displaySummaryButton, "Click to show summary about the order.");
            clearFormToolTip.SetToolTip(this.clearFormButton, "Click to clear the form and start new order.");
            exitToolTip.SetToolTip(this.exitButton, "Click to exit the form.");
        }
    }
}
