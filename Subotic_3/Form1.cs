//Name: Sasa Subotic
//Project: Assignment 3
//Date: 11/6/2018
//Description: Building a customer order form for some balloons!

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; //added the namespace for streamreader

namespace Subotic_3
{
    public partial class Form1 : Form
    {
        //declare class-level constants for tax rates and commison rates
        private const decimal HOME_DELIVERY_RATE = 7.50m;
        private const decimal SINGLE_BALLOON_RATE = 9.95m;
        private const decimal HALFDOZEN_BALLOON_RATE = 35.95m;
        private const decimal DOZEN_BALLOON_RATE = 65.95m;
        private const decimal EXTRAS_RATE = 9.50m;
        private const decimal MESSAGE_RATE = 0.00m;
        private const decimal MESSAGE_RATE_CHECKED = 2.50m;
        private const decimal SALES_TAX_RATE = 0.07m;

        private decimal subtotal = 0m;
        private decimal salesTax = 0m;
        private decimal orderTotal = 0m;
        private decimal deliveryTotal = 0m;
        private decimal bundleAmount = 0m;
        private decimal totalExtraItems = 0m;
        private int extrasCount = 0;
        private decimal personalmessagefee = 0m;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //displays current date
            deliveryDateMaskedTextBox.Text = DateTime.Now.ToString("MM/dd/yyyy");

            //Subtotal labels
            subTotalTotalLabel.Text = SINGLE_BALLOON_RATE.ToString("c");
            salesTaxTotalLabel.Text = (SINGLE_BALLOON_RATE*SALES_TAX_RATE).ToString("c");
            TotalLabel.Text = (SINGLE_BALLOON_RATE * SALES_TAX_RATE + SINGLE_BALLOON_RATE).ToString("c");

            //sets the label next to the bundles and the custom message totals to the appropriate prices
            homeDeliveryLabel.Text = MESSAGE_RATE.ToString("c");
            dozenLabel.Text = SINGLE_BALLOON_RATE.ToString("c");
            personalMessageTotalLabel.Text = MESSAGE_RATE.ToString("c");

            //Try-Catch Statement to catch input/output exception in the ComboBox for Occasions
            try
            {
                //sets the occasions combobox to read from the text file "occasions.txt" in the bin folder
                System.IO.StreamReader PopulateBoxes;
                occasionsComboBox.Items.Clear();                            // Clear any existing items from list        
                PopulateBoxes = System.IO.File.OpenText("occasions.txt");   // Open file        
                while (!PopulateBoxes.EndOfStream)                          // Verify that more data exists        
                {
                    // Read a line from the input file and add it to the list box            
                    occasionsComboBox.Items.Add(PopulateBoxes.ReadLine());
                }

                //closes the input file after reading the data
                PopulateBoxes.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //sets the combobox to "birthday" upon opening the program
            occasionsComboBox.SelectedIndex = 1;

            //Try-Catch Statement to catch input/output exceptions for the Extras List Box
            try
            {
                //sets the Extras listbox to read from the textfile "Extras.txt" in the bin folder
                System.IO.StreamReader PopulateBoxes;
                extrasListBox.Items.Clear();                        // Clear any existing items from list        
                PopulateBoxes = System.IO.File.OpenText("Extras.txt"); // Open file        
                while (!PopulateBoxes.EndOfStream)                     // Verify that more data exists        
                {
                    // Read a line from the input file and add it to the list box            
                    extrasListBox.Items.Add(PopulateBoxes.ReadLine());
                }

                //closes the output file after reading from data
                PopulateBoxes.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //disables personalized message until user clicks on the box
            messageTextBox.Enabled = false;
            messageTextBox.Text = "";
            noteLabel.Visible = false;

         
        }

        //This allows the total label under the ExtraListBox to be filled in automatically when a item is clicked upon
        private void extrasListBox_SelectedIndexChanged(object sender, EventArgs e)
        {   
            extrasCount = extrasListBox.SelectedItems.Count;
            totalExtraItems = Convert.ToDecimal(extrasCount * 9.50);
            extrasTotalLabel.Text = totalExtraItems.ToString("c");
            UpdateTotals(); //call custom method to calculate total
        }

        //This allows the controls for personalized messages to change depending on if the user wants a message or not
        private void messageCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (messageCheckBox.Checked)
            {
                noteLabel.Visible = true;
                messageTextBox.Enabled = true;
                personalMessageTotalLabel.Text = MESSAGE_RATE_CHECKED.ToString("c");
            }
            else
            {
                messageTextBox.Enabled = false;
                messageTextBox.Text = "";
                noteLabel.Visible = false;
                personalMessageTotalLabel.Text = MESSAGE_RATE.ToString("c");
            }
            UpdateTotals(); //call custom method to calculate total
        }

        //This changes the label with the total for a Delivery fee to 7.50 if HomeDelivery is checked, 0 otherwise.
        private void deliveryRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (deliveryRadioButton.Checked)
            {
                homeDeliveryLabel.Text = HOME_DELIVERY_RATE.ToString("c");
            }
            else
            {
                homeDeliveryLabel.Text = MESSAGE_RATE.ToString("c");
            }

            UpdateTotals(); //call custom method to calculate total
        }

        //Creating the UpdateTotals method to update the totals automatically
        private void UpdateTotals()
        {
            if (singleRadioButton.Checked || halfdozRadioButton.Checked || dozenRadioButton.Checked)
            {
                //calculate delivery price based on what radio button was clicked
                if (deliveryRadioButton.Checked)
                {
                    deliveryTotal = HOME_DELIVERY_RATE;
                }
                else if (pickupRadioButton.Checked)
                {
                    deliveryTotal = MESSAGE_RATE;
                }

                //Calculate bundle price based on what bundle was selected
                if (singleRadioButton.Checked)
                {
                    bundleAmount = SINGLE_BALLOON_RATE;
                }
                else if (halfdozRadioButton.Checked)
                {
                    bundleAmount = HALFDOZEN_BALLOON_RATE;
                }
                else 
                {
                    bundleAmount = DOZEN_BALLOON_RATE;
                }
            
                //Uses the totalExtraItems and extras count to subtotal the amount of extra items
                {
                    extrasCount = extrasListBox.SelectedItems.Count;
                    totalExtraItems = Convert.ToDecimal(extrasCount * 9.50); 
                }

                //Uses the Checkbox for the personal message to add the addtional 2.50 fee
                if (messageCheckBox.Checked)
                {
                    personalmessagefee = MESSAGE_RATE_CHECKED;
                }
                else
                {
                    personalmessagefee = MESSAGE_RATE;
                }

                //Calculate the subtotal here
                subtotal = personalmessagefee + totalExtraItems + bundleAmount + deliveryTotal;

                //Calculate sales tax amount
                salesTax = SALES_TAX_RATE * subtotal;

                //Calculate order total here
                orderTotal = salesTax + subtotal;

                //Format and display these amounts into currency formats and into the labeled totals
                subTotalTotalLabel.Text = subtotal.ToString("c");
                salesTaxTotalLabel.Text = salesTax.ToString("c");
                TotalLabel.Text = orderTotal.ToString("c");
            }
        }


        //a Method used to reset/clear the form when it is used
        private void ResetForm(Control.ControlCollection cc)
        {
            titleComboBox.Text = "";
            firstNameTextbox.Text = "";
            lastNameTextBox.Text = "";
            streetTextBox.Text = "";
            cityTextBox.Text = "";
            stateMaskedTextBox.Text = "";
            zipcodeMaskedTextBox.Text = "";
            phoneNumberMaskedTextBox.Text = "";
            deliveryDateMaskedTextBox.Text = "";
            pickupRadioButton.Checked = true;
            singleRadioButton.Checked = true;

        }
            

        private void singleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTotals(); //call custom method to calculate total
            if (singleRadioButton.Checked)
                {
                dozenLabel.Text = SINGLE_BALLOON_RATE.ToString("c");
                }
            else if (halfdozRadioButton.Checked)
                {
                dozenLabel.Text = HALFDOZEN_BALLOON_RATE.ToString("c");
                }
            else if (dozenRadioButton.Checked)
                {
                dozenLabel.Text = 65.95.ToString("c");
                }
        }

        private void summaryButton_Click(object sender, EventArgs e)
        {
            string deliverytype; //declare what delivery will be used in the message
            string bundle; //declare what bundles are used for the messagebox

            //For the delivery status
            if (deliveryRadioButton.Checked)
            {
                deliverytype = "Home Delivery";
            }
            else
            {
                deliverytype = "In-Store Pickup";
            }
           
            //For the Bundle Status
            if (singleRadioButton.Checked)
            {
                bundle = "Single Bundle";
            }
            else if(halfdozRadioButton.Checked)
            {
                bundle = "Half Dozen Bundle";
            }
            else
            {
                bundle = "Dozen Bundle";
            }

            //display messagebox with order summary
            MessageBox.Show("Bonnie's Balloons Order Summary!" + "\n" +
                             "Customer Name: " + titleComboBox.Text + " " + firstNameTextbox.Text + " " + lastNameTextBox.Text + "\n" +
                             "Customer Address: " + streetTextBox.Text + "," + cityTextBox.Text + "," + stateMaskedTextBox.Text + "," + zipcodeMaskedTextBox.Text + "\n" +
                             "Customer Phone Number: " + phoneNumberMaskedTextBox.Text + "\n" +
                             "Delivery Date: " + deliveryDateMaskedTextBox.Text + "\n" +
                             "Delivery Type: " + deliverytype + "\n" +
                             "Bundle Size: " + bundle + "\n" +
                             "Occasions: " + occasionsComboBox.Text + "\n" +
                             "Extras: " + extrasListBox.Text + "\n" +
                             "Message: " + messageTextBox.Text + "\n" +
                             "Order Subtotal:" + subTotalTotalLabel.Text + "\n" +
                             "Sales Tax: " + salesTaxTotalLabel.Text + "\n" +
                             "Order Total: " + TotalLabel.Text

                             );
            
        }

        private void clearButton_Click(object sender, EventArgs e)
        {

        }
    }
}
