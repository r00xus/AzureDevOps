﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MDS.Azure.DevOps.Reader.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class WIQL {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal WIQL() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MDS.Azure.DevOps.Reader.Resources.WIQL", typeof(WIQL).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///  *
        ///FROM
        ///  WorkItems
        ///WHERE
        ///  [Team Project] = &apos;UIS&apos; AND
        ///  [Work Item Type] = &apos;Activity&apos; AND
        ///  [Assigned To] IN (@persons) AND
        ///  [Target Date] &gt;= &apos;@dateFrom&apos; AND
        ///  [Target Date] &lt;= &apos;@dateTo&apos;.
        /// </summary>
        internal static string GetActivity {
            get {
                return ResourceManager.GetString("GetActivity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///	*
        ///FROM
        ///	WorkItemLinks
        ///WHERE	
        ///	[Source].[System.WorkItemType] = &apos;Activity&apos; AND
        ///	[Target].[System.WorkItemType] = &apos;Task&apos; AND
        ///	[Source].[System.Id] IN (@activityId).
        /// </summary>
        internal static string GetTaskLinks {
            get {
                return ResourceManager.GetString("GetTaskLinks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///  *
        ///FROM
        ///  WorkItems
        ///WHERE
        ///  [System.Id] IN (@id).
        /// </summary>
        internal static string GetTasks {
            get {
                return ResourceManager.GetString("GetTasks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///  *
        ///FROM
        ///  WorkItems
        ///WHERE
        ///  [System.Id] IN (@id).
        /// </summary>
        internal static string GetTasksById {
            get {
                return ResourceManager.GetString("GetTasksById", resourceCulture);
            }
        }
    }
}
